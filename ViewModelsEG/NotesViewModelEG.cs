using CommunityToolkit.Mvvm.Input;
using MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MVVM.ViewModelsEG;

internal class NotesViewModelEG : IQueryAttributable
{
    public ObservableCollection<NoteViewModelEG> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public NotesViewModelEG()
    {
        AllNotes = new ObservableCollection<NoteViewModelEG>(NoteEG.LoadAll().Select(n => new NoteViewModelEG(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<NoteViewModelEG>(SelectNoteAsync);
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task SelectNoteAsync(NoteViewModelEG note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            NoteViewModelEG matchedNote = AllNotes.FirstOrDefault(n => n.Identifier == noteId);

            // Si la nota existe, elimínala
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            NoteViewModelEG matchedNote = AllNotes.FirstOrDefault(n => n.Identifier == noteId);

            // Si la nota se encuentra, actualízala
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            // Si la nota no se encuentra, agrégala como nueva al inicio
            else
                AllNotes.Insert(0, new NoteViewModelEG(NoteEG.Load(noteId)));
        }
    }
}
