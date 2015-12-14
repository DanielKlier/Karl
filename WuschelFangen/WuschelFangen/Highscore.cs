using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace WuschelFangen
{
    public class Highscore
    {
        public struct Entry
        {
            public float Score;
        }

        private readonly List<Entry> _entries = new List<Entry>();

        private Highscore()
        {
        }

        static Highscore()
        {
            Instance = new Highscore();
        }

        public static Highscore Instance { get; private set; }

        public void InsertEntry(Entry entry)
        {
            var inserted = false;

            for (var idxEntry = 0; idxEntry < _entries.Count; ++idxEntry)
            {
                var currEntry = _entries[idxEntry];

                if (!(entry.Score > currEntry.Score)) continue;

                _entries.Insert(idxEntry, entry);
                inserted = true;

                break;
            }

            if (!inserted)
                _entries.Add(entry);

            if (_entries.Count > 5)
                _entries.RemoveRange(5, _entries.Count - 5);
        }

        public void Load(string filename)
        {
            if (!File.Exists(filename))
                return;

            using (var reader = new StreamReader(filename))
            {
                for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var entry = new Entry();

                    var idxFirstSpace = line.IndexOf(' ');
                    var score = (idxFirstSpace < 0) ? line : line.Substring(0, idxFirstSpace);

                    if (!float.TryParse(score, NumberStyles.Float, CultureInfo.InvariantCulture, out entry.Score))
                        continue;

                    InsertEntry(entry);
                }
            }
        }

        public void Save(string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                foreach (var entry in _entries)
                {
                    writer.WriteLine(entry.Score.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        public IList<Entry> Entries
        {
            get { return _entries.AsReadOnly(); }
        }
    }
}
