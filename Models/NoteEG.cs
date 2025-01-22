using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Models;

internal class NoteEG
{
    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public NoteEG()
    {
        Text = "";
        Date = DateTime.Now;
        Filename = $"{Path.GetRandomFileName()}.notes.txt";
    }

    public void Save() =>
        File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Text);

    public void Delete() =>
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));

    public static NoteEG Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return new NoteEG
        {
            Filename = Path.GetFileName(filename),
            Text = File.ReadAllText(filename),
            Date = File.GetLastWriteTime(filename)
        };
    }

    public static IEnumerable<NoteEG> LoadAll()
    {
        string appDataPath = FileSystem.AppDataDirectory;

        return Directory
            .EnumerateFiles(appDataPath, "*.notes.txt")
            .Select(filename => NoteEG.Load(Path.GetFileName(filename)))
            .OrderByDescending(note => note.Date);
    }
}