using CBRE.DataStructures.GameData;
using CBRE.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CBRE.Providers.GameData
{
    public class FgdProvider : GameDataProvider
    {
        private String CurrentFile { get; set; }

        protected override bool IsValidForFile(string filename)
        {
            return filename.EndsWith(".fgd", StringComparison.OrdinalIgnoreCase);
        }

        protected override bool IsValidForStream(Stream stream)
        {
            // not really any way of knowing
            return true;
        }

        protected override DataStructures.GameData.GameData GetFromFile(string filename)
        {
            if (!File.Exists(filename)) throw new ProviderException(Local.LocalString("exception.file_not_exist", filename));
            CurrentFile = filename;
            DataStructures.GameData.GameData parsed = base.GetFromFile(filename);
            CurrentFile = null;
            return parsed;
        }

        protected override DataStructures.GameData.GameData GetFromStream(Stream stream)
        {
            IEnumerable<LexObject> lex = Lex(new StreamReader(stream));
            return Parse(lex.Where(l => l.Type != LexType.Comment));
        }

        private DataStructures.GameData.GameData Parse(IEnumerable<LexObject> lex)
        {
            DataStructures.GameData.GameData gd = new DataStructures.GameData.GameData();
            IEnumerator<LexObject> iterator = lex.GetEnumerator();
            while (true)
            {
                if (!iterator.MoveNext()) break;
                if (iterator.Current.Type == LexType.At)
                {
                    ParseAt(gd, iterator);
                }
            }
            return gd;
        }

        private void ParseAt(DataStructures.GameData.GameData gd, IEnumerator<LexObject> iterator)
        {
            iterator.MoveNext();
            string type = iterator.Current.Value;
            if (type.Equals("include", StringComparison.OrdinalIgnoreCase))
            {
                Expect(iterator, LexType.String);
                if (CurrentFile != null)
                {
                    string filename = iterator.Current.GetValue();
                    string path = Path.GetDirectoryName(CurrentFile) ?? "";
                    string incfile = Path.Combine(path, filename);

                    string current = CurrentFile;
                    DataStructures.GameData.GameData incgd = GetGameDataFromFile(incfile);
                    CurrentFile = current;

                    if (!gd.Includes.Any(x => String.Equals(x, filename, StringComparison.OrdinalIgnoreCase))) gd.Includes.Add(filename);

                    // Merge the included gamedata into the current one
                    gd.MapSizeHigh = Math.Max(incgd.MapSizeHigh, gd.MapSizeHigh);
                    gd.MapSizeLow = Math.Min(incgd.MapSizeLow, gd.MapSizeLow);
                    gd.Includes.AddRange(incgd.Includes.Where(x => !gd.Includes.Contains(x)));
                    gd.Classes.AddRange(incgd.Classes.Where(x => !gd.Classes.Any(y => String.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase))));
                    gd.AutoVisgroups.AddRange(incgd.AutoVisgroups.Where(x => !gd.AutoVisgroups.Any(y => String.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase))));
                    gd.MaterialExclusions.AddRange(incgd.MaterialExclusions.Where(x => !gd.MaterialExclusions.Any(y => String.Equals(x, y, StringComparison.OrdinalIgnoreCase))));
                }
                else
                {
                    throw new ProviderException(Local.LocalString("exception.unable_include_when_not_reading"));
                }
            }
            else if (type.Equals("mapsize", StringComparison.OrdinalIgnoreCase))
            {
                Expect(iterator, LexType.OpenParen);
                Expect(iterator, LexType.Value);
                gd.MapSizeLow = Int32.Parse(iterator.Current.Value);
                Expect(iterator, LexType.Comma);
                Expect(iterator, LexType.Value);
                gd.MapSizeHigh = Int32.Parse(iterator.Current.Value);
                Expect(iterator, LexType.CloseParen);
            }
            else if (type.Equals("materialexclusion", StringComparison.OrdinalIgnoreCase))
            {
                Expect(iterator, LexType.OpenBracket);
                iterator.MoveNext();
                while (iterator.Current.Type != LexType.CloseBracket)
                {
                    Assert(iterator.Current, iterator.Current.IsValueOrString(), Local.LocalString("exception.expected_value_type", iterator.Current.Type));
                    string exclusion = iterator.Current.GetValue();
                    gd.MaterialExclusions.Add(exclusion);
                    iterator.MoveNext();
                }
            }
            else if (type.Equals("autovisgroup", StringComparison.OrdinalIgnoreCase))
            {
                Expect(iterator, LexType.Equals);

                iterator.MoveNext();
                Assert(iterator.Current, iterator.Current.IsValueOrString(), Local.LocalString("exception.expected_value_type", iterator.Current.Type));
                string sectionName = iterator.Current.GetValue();
                AutoVisgroupSection sect = new AutoVisgroupSection { Name = sectionName };

                Expect(iterator, LexType.OpenBracket);
                iterator.MoveNext();
                while (iterator.Current.Type != LexType.CloseBracket)
                {
                    Assert(iterator.Current, iterator.Current.IsValueOrString(), Local.LocalString("exception.expected_value_type", iterator.Current.Type));
                    string groupName = iterator.Current.GetValue();
                    AutoVisgroup grp = new AutoVisgroup { Name = groupName };

                    Expect(iterator, LexType.OpenBracket);
                    iterator.MoveNext();
                    while (iterator.Current.Type != LexType.CloseBracket)
                    {
                        Assert(iterator.Current, iterator.Current.IsValueOrString(), Local.LocalString("exception.expected_value_type", iterator.Current.Type));
                        string entity = iterator.Current.GetValue();
                        grp.EntityNames.Add(entity);
                        iterator.MoveNext();
                    }

                    sect.Groups.Add(grp);
                }

                gd.AutoVisgroups.Add(sect);
            }
            else
            {
                // Parsing:
                // @TypeClass name(param, param) name()
                ClassType ct = ParseClassType(type, iterator.Current);
                GameDataObject gdo = new GameDataObject("", "", ct);
                iterator.MoveNext();
                while (iterator.Current.Type == LexType.Value)
                {
                    // Parsing:
                    // @TypeClass {name(param, param) name()}
                    string name = iterator.Current.Value;
                    Behaviour bh = new Behaviour(name);
                    iterator.MoveNext();
                    if (iterator.Current.Type == LexType.Value)
                    {
                        // Allow for the following (first seen in hl2 base):
                        // @PointClass {halfgridsnap} base(Targetname)
                        continue;
                    }
                    Assert(iterator.Current, iterator.Current.Type == LexType.OpenParen, Local.LocalString("exception.unexpected", iterator.Current.Type));
                    iterator.MoveNext();
                    while (iterator.Current.Type != LexType.CloseParen)
                    {
                        // Parsing:
                        // name({param, param})
                        if (iterator.Current.Type != LexType.Comma)
                        {
                            Assert(iterator.Current, iterator.Current.Type == LexType.Value || iterator.Current.Type == LexType.String,
                                Local.LocalString("exception.unexpected", iterator.Current.Type));
                            string value = iterator.Current.Value;
                            if (iterator.Current.Type == LexType.String) value = value.Trim('"');
                            bh.Values.Add(value);
                        }
                        iterator.MoveNext();
                    }
                    Assert(iterator.Current, iterator.Current.Type == LexType.CloseParen, Local.LocalString("exception.unexpected", iterator.Current.Type));
                    // Treat base behaviour as a special case
                    if (bh.Name == "base")
                    {
                        gdo.BaseClasses.AddRange(bh.Values);
                    }
                    else
                    {
                        gdo.Behaviours.Add(bh);
                    }
                    iterator.MoveNext();
                }
                // = class_name : "Descr" + "iption" [
                Assert(iterator.Current, iterator.Current.Type == LexType.Equals, Local.LocalString("exception.expected_equals", iterator.Current.Type));
                Expect(iterator, LexType.Value);
                gdo.Name = iterator.Current.Value;
                iterator.MoveNext();
                if (iterator.Current.Type == LexType.Colon)
                {
                    // Parsing:
                    // : {"Descr" + "iption"} [
                    iterator.MoveNext();
                    gdo.Description = ParsePlusString(iterator);
                }
                Assert(iterator.Current, iterator.Current.Type == LexType.OpenBracket, Local.LocalString("exception.unexpected", iterator.Current.Type));

                // Parsing:
                // name(type) : "Desc" : "Default" : "Long Desc" = [ ... ]
                // input name(type) : "Description"
                // output name(type) : "Description"
                iterator.MoveNext();
                while (iterator.Current.Type != LexType.CloseBracket)
                {
                    Assert(iterator.Current, iterator.Current.Type == LexType.Value, Local.LocalString("exception.unexpected", iterator.Current.Type));
                    string pt = iterator.Current.Value;
                    if (pt == "input" || pt == "output") // IO
                    {
                        // input name(type) : "Description"
                        IO io = new IO();
                        Expect(iterator, LexType.Value);
                        io.IOType = (IOType)Enum.Parse(typeof(IOType), pt, true);
                        io.Name = iterator.Current.Value;
                        Expect(iterator, LexType.OpenParen);
                        Expect(iterator, LexType.Value);
                        io.VariableType = ParseVariableType(iterator.Current);
                        Expect(iterator, LexType.CloseParen);
                        iterator.MoveNext(); // if not colon, this will be the value of the next io/property, or close
                        if (iterator.Current.Type == LexType.Colon)
                        {
                            iterator.MoveNext();
                            io.Description = ParsePlusString(iterator);
                        }
                        gdo.InOuts.Add(io);
                    }
                    else // Property
                    {
                        Expect(iterator, LexType.OpenParen);
                        Expect(iterator, LexType.Value);
                        VariableType vartype = ParseVariableType(iterator.Current);
                        Expect(iterator, LexType.CloseParen);
                        Property prop = new Property(pt, vartype);
                        iterator.MoveNext();
                        // if not colon or equals, this will be the value of the next io/property, or close
                        if (iterator.Current.Type == LexType.Value)
                        {
                            // Check for additional flags on the property
                            // e.g.: name(type) readonly : "This is a read only value"
                            //       name(type) report   : "This value will show in the entity report"
                            switch (iterator.Current.Value)
                            {
                                case "readonly":
                                    prop.ReadOnly = true;
                                    iterator.MoveNext();
                                    break;
                                case "report":
                                    prop.ShowInEntityReport = true;
                                    iterator.MoveNext();
                                    break;
                            }
                        }
                        do // Using do/while(false) so I can break out - reduces nesting.
                        {
                            // Short description
                            if (iterator.Current.Type != LexType.Colon) break;
                            iterator.MoveNext();
                            prop.ShortDescription = ParsePlusString(iterator);

                            // Default value
                            if (iterator.Current.Type != LexType.Colon) break;
                            iterator.MoveNext();
                            if (iterator.Current.Type != LexType.Colon) // Allow for ': :' structure (no default)
                            {
                                if (iterator.Current.Type == LexType.String)
                                {
                                    prop.DefaultValue = iterator.Current.Value.Trim('"');
                                }
                                else
                                {
                                    Assert(iterator.Current, iterator.Current.Type == LexType.Value, Local.LocalString("exception.unexpected", iterator.Current.Type));
                                    prop.DefaultValue = iterator.Current.Value;
                                }
                                iterator.MoveNext();
                            }

                            // Long description
                            if (iterator.Current.Type != LexType.Colon) break;
                            iterator.MoveNext();
                            prop.Description = ParsePlusString(iterator);
                        } while (false);
                        if (iterator.Current.Type == LexType.Equals)
                        {
                            Expect(iterator, LexType.OpenBracket);
                            // Parsing property options:
                            // value : description
                            // value : description : 0
                            iterator.MoveNext();
                            while (iterator.Current.IsValueOrString())
                            {
                                Option opt = new Option
                                {
                                    Key = iterator.Current.GetValue()
                                };
                                Expect(iterator, LexType.Colon);

                                // Some FGDs use values for property descriptions instead of strings
                                iterator.MoveNext();
                                Assert(iterator.Current, iterator.Current.IsValueOrString(), Local.LocalString("exception.must_be_value_string"));
                                if (iterator.Current.Type == LexType.String)
                                {
                                    opt.Description = ParsePlusString(iterator);
                                }
                                else
                                {
                                    opt.Description = iterator.Current.GetValue();
                                    iterator.MoveNext();
                                    // ParsePlusString moves next once it's complete, need to do the same here
                                }

                                prop.Options.Add(opt);
                                if (iterator.Current.Type != LexType.Colon)
                                {
                                    continue;
                                }
                                Expect(iterator, LexType.Value);
                                opt.On = iterator.Current.Value == "1";
                                iterator.MoveNext();
                            }
                            Assert(iterator.Current, iterator.Current.Type == LexType.CloseBracket, Local.LocalString("exception.unexpected", iterator.Current.Type));
                            iterator.MoveNext();
                        }
                        gdo.Properties.Add(prop);
                    }
                }
                Assert(iterator.Current, iterator.Current.Type == LexType.CloseBracket, Local.LocalString("exception.unexpected", iterator.Current.Type));
                gd.Classes.Add(gdo);
            }
        }

        /// <summary>
        /// Parse the iterator's tokens until the current token is not a plus or a string.
        /// </summary>
        /// <param name="iterator">A token iterator, the current value should be the start of the string to parse.</param>
        /// <returns>The string result.</returns>
        private static string ParsePlusString(IEnumerator<LexObject> iterator)
        {
            string result = "";
            bool plustime = false;
            while (iterator.Current.Type == LexType.String || iterator.Current.Type == LexType.Plus)
            {
                if (iterator.Current.Type != LexType.Plus)
                {
                    if (plustime) break;
                    Assert(iterator.Current, iterator.Current.Type == LexType.String, Local.LocalString("exception.unexpected", iterator.Current.Type));
                    result += iterator.Current.Value.Trim('"');
                }
                else
                {
                    if (!plustime) break;
                }
                plustime = !plustime;
                iterator.MoveNext();
            }
            return result;
        }

        private static ClassType ParseClassType(string type, LexObject obj)
        {
            type = type.ToLower().Replace("class", "");
            ClassType ct;
            if (Enum.TryParse(type, true, out ct))
            {
                return ct;
            }
            throw new ProviderException(Local.LocalString("exception.unable_parse_fgd_class", type, obj.LineNumber, obj.CharacterNumber));
        }

        private static VariableType ParseVariableType(LexObject obj)
        {
            string type = obj.Value.ToLower().Replace("_", "");
            VariableType vt;
            if (Enum.TryParse(type, true, out vt))
            {
                return vt;
            }
            throw new ProviderException(Local.LocalString("exception.unable_parse_fgd_variable", type, obj.LineNumber, obj.CharacterNumber));
        }

        private static void Expect(IEnumerator<LexObject> iterator, LexType lexType)
        {
            iterator.MoveNext();
            if (iterator.Current.Type != lexType)
            {
                throw new ProviderException(Local.LocalString("exception.unable_parse_fgd_lex", lexType, iterator.Current.Type, iterator.Current.LineNumber, iterator.Current.CharacterNumber));
            }
        }

        private static void Assert(LexObject obj, bool value, string error)
        {
            if (!value)
            {
                throw new ProviderException(Local.LocalString("exception.unable_parse_fgd", error.Trim(), obj.LineNumber, obj.CharacterNumber));
            }
        }

        private enum LexType
        {
            At,             // @
            Equals,         // =
            Colon,          // :
            OpenBracket,    // [
            CloseBracket,   // ]
            OpenParen,      // (
            CloseParen,     // )
            Plus,           // +
            Comma,          // ,
            Value,          // Any unquoted string not in the above list
            String,         // Any quoted string (including quotes)
            Comment
        }

        private class LexObject
        {
            public LexType Type { get; private set; }
            public string Value { get; set; }
            public int LineNumber { get; private set; }
            public int CharacterNumber { get; private set; }

            public LexObject(int lineNum, int charNum, LexType type, string value = "")
            {
                LineNumber = lineNum;
                CharacterNumber = charNum;
                Type = type;
                Value = value;
            }

            public bool IsValueOrString()
            {
                return Type == LexType.String || Type == LexType.Value;
            }

            public string GetValue()
            {
                return Type == LexType.String ? Value.Trim('"') : Value;
            }
        }

        private static IEnumerable<LexObject> Lex(TextReader reader)
        {
            int lineNum = 1;
            int charNum = 0;
            int i;
            LexObject current = null;
            while ((i = reader.Read()) >= 0)
            {
                char c = Convert.ToChar(i);
                if (c == '\n')
                {
                    lineNum++;
                    charNum = 0;
                }
                else
                {
                    charNum++;
                }
                if (current == null)
                {
                    current = LexNew(lineNum, charNum, c);
                }
                else
                {
                    LexObject le = LexExisting(lineNum, charNum, c, current);
                    if (le != current)
                    {
                        yield return current;
                        current = le;
                    }
                }
            }
            if (current != null)
            {
                yield return current;
            }
        }

        private static LexObject LexNew(int lineNum, int charNum, char c)
        {
            if (Char.IsWhiteSpace(c))
            {
                return null;
            }
            if (c == '@')
            {
                return new LexObject(lineNum, charNum, LexType.At);
            }
            if (c == '=')
            {
                return new LexObject(lineNum, charNum, LexType.Equals);
            }
            if (c == ':')
            {
                return new LexObject(lineNum, charNum, LexType.Colon);
            }
            if (c == '[')
            {
                return new LexObject(lineNum, charNum, LexType.OpenBracket);
            }
            if (c == ']')
            {
                return new LexObject(lineNum, charNum, LexType.CloseBracket);
            }
            if (c == '(')
            {
                return new LexObject(lineNum, charNum, LexType.OpenParen);
            }
            if (c == ')')
            {
                return new LexObject(lineNum, charNum, LexType.CloseParen);
            }
            if (c == '+')
            {
                return new LexObject(lineNum, charNum, LexType.Plus);
            }
            if (c == ',')
            {
                return new LexObject(lineNum, charNum, LexType.Comma);
            }
            if (c == '/')
            {
                return new LexObject(lineNum, charNum, LexType.Comment);
            }
            if (c == '"')
            {
                return new LexObject(lineNum, charNum, LexType.String, c.ToString());
            }
            return new LexObject(lineNum, charNum, LexType.Value, c.ToString());
        }

        private static readonly char[] NonValueCharacters =
        {
            '@', '=', ':', '[', ']', '(', ')', '+', ','
        };

        private static LexObject LexExisting(int lineNum, int charNum, char c, LexObject existing)
        {
            switch (existing.Type)
            {
                case LexType.Value:
                    if (Char.IsWhiteSpace(c))
                    {
                        return null;
                    }
                    if (NonValueCharacters.Contains(c))
                    {
                        return LexNew(lineNum, charNum, c);
                    }
                    existing.Value += c.ToString();
                    return existing;
                case LexType.String:
                    existing.Value += c.ToString();
                    return c == '"' ? null : existing;
                case LexType.Comment:
                    return c == '\n' ? null : existing;
                default:
                    return LexNew(lineNum, charNum, c);
            }
        }
    }
}
