/*=======================================================================
  Copyright (C) Microsoft Corporation.  All rights reserved.

  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
  PARTICULAR PURPOSE.
=======================================================================*/

using System;
using System.Collections.Generic;

namespace RssToolkit
{
    /// <summary>
    /// Used to convert to Plural or Singular
    /// </summary>
    public static class Pluralizer
    {
        static Dictionary<string, Word> _specialSingulars;
        static Dictionary<string, Word> _specialPlurals;
        static List<SuffixRule> _suffixRules;

        #region Special Words Table
        private static string[] _specialWordsStringTable = new string[]
        {
            "agendum",          "agenda", string.Empty,
            "albino",           "albinos", string.Empty,
            "alga",             "algae", string.Empty,
            "alumna",           "alumnae", string.Empty,
            "apex",             "apices",           "apexes",
            "archipelago",      "archipelagos", string.Empty,
            "bacterium",        "bacteria", string.Empty,
            "beef",             "beefs",            "beeves",
            "bison", string.Empty, string.Empty,
            "brother",          "brothers",         "brethren",
            "candelabrum",      "candelabra", string.Empty,
            "carp", string.Empty, string.Empty,
            "casino",           "casinos", string.Empty,
            "child",            "children", string.Empty,
            "chassis", string.Empty, string.Empty,
            "chinese", string.Empty, string.Empty,
            "clippers", string.Empty, string.Empty,
            "cod", string.Empty, string.Empty,
            "codex",            "codices", string.Empty,
            "commando",         "commandos", string.Empty,
            "corps", string.Empty, string.Empty,
            "cortex",           "cortices",         "cortexes",
            "cow",              "cows",             "kine",
            "criterion",        "criteria", string.Empty,
            "datum",            "data", string.Empty,
            "debris", string.Empty, string.Empty,
            "diabetes", string.Empty, string.Empty,
            "ditto",            "dittos", string.Empty,
            "djinn", string.Empty, string.Empty,
            "dynamo", string.Empty, string.Empty,
            "elk", string.Empty, string.Empty,
            "embryo",           "embryos", string.Empty,
            "ephemeris",        "ephemeris",        "ephemerides",
            "erratum",          "errata", string.Empty,
            "extremum",         "extrema", string.Empty,
            "fiasco",           "fiascos", string.Empty,
            "fish",             "fishes",           "fish",
            "flounder", string.Empty, string.Empty,
            "focus",            "focuses",          "foci",
            "fungus",           "fungi",            "funguses",
            "gallows", string.Empty, string.Empty,
            "genie",            "genies",           "genii",
            "ghetto",           "ghettos", string.Empty,
            "graffiti", string.Empty, string.Empty,
            "headquarters", string.Empty, string.Empty,
            "herpes", string.Empty, string.Empty,
            "homework", string.Empty, string.Empty,
            "index",            "indices",          "indexes",
            "inferno",          "infernos", string.Empty,
            "japanese", string.Empty, string.Empty,
            "jumbo",            "jumbos", string.Empty,
            "latex",            "latices",          "latexes",
            "lingo",            "lingos", string.Empty,
            "mackerel", string.Empty, string.Empty,
            "macro",            "macros", string.Empty,
            "manifesto",        "manifestos", string.Empty,
            "measles", string.Empty, string.Empty,
            "money",            "moneys",           "monies",
            "mongoose",         "mongooses",        "mongoose",
            "mumps", string.Empty, string.Empty,
            "murex",            "murecis", string.Empty,
            "mythos",           "mythos",           "mythoi",
            "news", string.Empty, string.Empty,
            "octopus",          "octopuses",        "octopodes",
            "ovum",             "ova", string.Empty,
            "ox",               "ox",               "oxen",
            "photo",            "photos", string.Empty,
            "pincers", string.Empty, string.Empty,
            "pliers", string.Empty, string.Empty,
            "pro",              "pros", string.Empty,
            "rabies", string.Empty, string.Empty,
            "radius",           "radiuses",         "radii",
            "rhino",            "rhinos", string.Empty,
            "salmon", string.Empty, string.Empty,
            "scissors", string.Empty, string.Empty,
            "series", string.Empty, string.Empty,
            "shears", string.Empty, string.Empty,
            "silex",            "silices", string.Empty,
            "simplex",          "simplices",        "simplexes",
            "soliloquy",        "soliloquies",      "soliloquy",
            "species", string.Empty, string.Empty,
            "stratum",          "strata", string.Empty,
            "swine", string.Empty, string.Empty,
            "trout", string.Empty, string.Empty,
            "tuna", string.Empty, string.Empty,
            "vertebra",         "vertebrae", string.Empty,
            "vertex",           "vertices",         "vertexes",
            "vortex",           "vortices",         "vortexes",
        };
        #endregion

        #region Suffix Rules Table
        private static string[] _suffixRulesStringTable = new string[]
        {
            "ch",       "ches",
            "sh",       "shes",
            "ss",       "sses",

            "ay",       "ays",
            "ey",       "eys",
            "iy",       "iys",
            "oy",       "oys",
            "uy",       "uys",
            "y",        "ies",

            "ao",       "aos",
            "eo",       "eos",
            "io",       "ios",
            "oo",       "oos",
            "uo",       "uos",
            "o",        "oes",

            "cis",      "ces",
            "sis",      "ses",
            "xis",      "xes",

            "louse",    "lice",
            "mouse",    "mice",

            "zoon",     "zoa",

            "man",      "men",

            "deer",     "deer",
            "fish",     "fish",
            "sheep",    "sheep",
            "itis",     "itis",
            "ois",      "ois",
            "pox",      "pox",
            "ox",       "oxes",

            "foot",     "feet",
            "goose",    "geese",
            "tooth",    "teeth",

            "alf",      "alves",
            "elf",      "elves",
            "olf",      "olves",
            "arf",      "arves",
            "leaf",     "leaves",
            "nife",     "nives",
            "life",     "lives",
            "wife",     "wives",
        };
        #endregion

        #region public APIs
        /// <summary>
        /// Initializes the <see cref="Pluralizer"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Pluralizer()
        {
            // populate lookup tables for special words
            _specialSingulars = new Dictionary<string, Word>(StringComparer.OrdinalIgnoreCase);
            _specialPlurals = new Dictionary<string, Word>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < _specialWordsStringTable.Length; i += 3)
            {
                string s = _specialWordsStringTable[i];
                string p = _specialWordsStringTable[i + 1];
                string p2 = _specialWordsStringTable[i + 2];

                if (string.IsNullOrEmpty(p))
                {
                    p = s;
                }

                Word w = new Word(s, p);

                _specialSingulars.Add(s, w);
                _specialPlurals.Add(p, w);

                if (!string.IsNullOrEmpty(p2))
                {
                    _specialPlurals.Add(p2, w);
                }
            }

            // populate suffix rules list
            _suffixRules = new List<SuffixRule>();

            for (int i = 0; i < _suffixRulesStringTable.Length; i += 2)
            {
                string singular = _suffixRulesStringTable[i];
                string plural = _suffixRulesStringTable[i + 1];
                _suffixRules.Add(new SuffixRule(singular, plural));
            }
        }

        /// <summary>
        /// Converts to the plural.
        /// </summary>
        /// <param name="noun">The word.</param>
        /// <returns>plural form of the word</returns>
        public static string ToPlural(string noun)
        {
            return AdjustCase(ToPluralInternal(noun), noun);
        }

        /// <summary>
        /// Converts to the singular.
        /// </summary>
        /// <param name="noun">The word.</param>
        /// <returns>singular form of the word</returns>
        public static string ToSingular(string noun)
        {
            return AdjustCase(ToSingularInternal(noun), noun);
        }

        /// <summary>
        /// Determines whether [is noun plural of noun] [the specified plural].
        /// </summary>
        /// <param name="plural">The plural.</param>
        /// <param name="singular">The singular.</param>
        /// <returns>
        /// 	<c>true</c> if [is noun plural of noun] [the specified plural]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNounPluralOfNoun(string plural, string singular)
        {
            return String.Compare(ToSingularInternal(plural), singular, StringComparison.OrdinalIgnoreCase) == 0;
        }
        #endregion

        #region Implementation Details
        private static string ToPluralInternal(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            // lookup special words
            Word word;

            if (_specialSingulars.TryGetValue(s, out word))
            {
                return word.Plural;
            }

            // apply suffix rules
            foreach (SuffixRule rule in _suffixRules)
            {
                string plural;
                if (rule.TryToPlural(s, out plural))
                {
                    return plural;
                }
            }

            // apply the default rule
            return s + "s";
        }

        private static string ToSingularInternal(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            // lookup special words
            Word word;

            if (_specialPlurals.TryGetValue(s, out word))
            {
                return word.Singular;
            }

            // apply suffix rules
            foreach (SuffixRule rule in _suffixRules)
            {
                string singular;
                if (rule.TryToSingular(s, out singular))
                {
                    return singular;
                }
            }

            // apply the default rule
            if (s.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                return s.Substring(0, s.Length - 1);
            }

            return s;
        }

        private static string AdjustCase(string s, string template)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            // determine the type of casing of the template string
            bool foundUpperOrLower = false;
            bool allLower = true;
            bool allUpper = true;
            bool firstUpper = false;

            for (int i = 0; i < template.Length; i++)
            {
                if (Char.IsUpper(template[i]))
                {
                    if (i == 0)
                        firstUpper = true;
                    allLower = false;
                    foundUpperOrLower = true;
                }
                else if (Char.IsLower(template[i]))
                {
                    allUpper = false;
                    foundUpperOrLower = true;
                }
            }

            // change the case according to template
            if (foundUpperOrLower)
            {
                if (allLower)
                {
                    s = s.ToLowerInvariant();
                }
                else if (allUpper)
                {
                    s = s.ToUpperInvariant();
                }
                else if (firstUpper)
                {
                    if (!Char.IsUpper(s[0]))
                    {
                        s = s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
                    }
                }
            }

            return s;
        }
        #endregion

        /// <summary>
        /// Suffix rule
        /// </summary>
        private class SuffixRule
        {
            private readonly string _singularSuffix;
            private readonly string _pluralSuffix;

            /// <summary>
            /// Initializes a new instance of the <see cref="SuffixRule"/> class.
            /// </summary>
            /// <param name="singular">The singular.</param>
            /// <param name="plural">The plural.</param>
            public SuffixRule(string singular, string plural)
            {
                if (string.IsNullOrEmpty(singular))
                {
                    throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "singular"));
                }

                if (string.IsNullOrEmpty(plural))
                {
                    throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "plural"));
                }

                _singularSuffix = singular;
                _pluralSuffix = plural;
            }

            /// <summary>
            /// Tries to plural.
            /// </summary>
            /// <param name="word">The word.</param>
            /// <param name="plural">The plural.</param>
            /// <returns>bool</returns>
            public bool TryToPlural(string word, out string plural)
            {
                if (string.IsNullOrEmpty(word))
                {
                    throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "word"));
                }

                if (word.EndsWith(_singularSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    plural = word.Substring(0, word.Length - _singularSuffix.Length) + _pluralSuffix;
                    return true;
                }
                else
                {
                    plural = null;
                    return false;
                }
            }

            /// <summary>
            /// Tries to singular.
            /// </summary>
            /// <param name="word">The word.</param>
            /// <param name="singular">The singular.</param>
            /// <returns>bool</returns>
            public bool TryToSingular(string word, out string singular)
            {
                if (string.IsNullOrEmpty(word))
                {
                    throw new ArgumentException(string.Format(Resources.RssToolkit.Culture, Resources.RssToolkit.ArgmentException, "word"));
                }

                if (word.EndsWith(_pluralSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    singular = word.Substring(0, word.Length - _pluralSuffix.Length) + _singularSuffix;
                    return true;
                }
                else
                {
                    singular = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Word
        /// </summary>
        private class Word
        {
            /// <summary>
            /// String representing Singular
            /// </summary>
            internal readonly string Singular;

            /// <summary>
            /// String representing Plural
            /// </summary>
            internal readonly string Plural;

            /// <summary>
            /// Initializes a new instance of the <see cref="Word"/> class.
            /// </summary>
            /// <param name="singular">The singular.</param>
            /// <param name="plural">The plural.</param>
            /// <param name="plural2">The plural2.</param>
            public Word(string singular, string plural)
            {
                if (singular == null)
                {
                    throw new ArgumentNullException("singular");
                }

                if (plural == null)
                {
                    throw new ArgumentNullException("plural");
                }

                Singular = singular;
                Plural = plural;
            }
        }
    }
}