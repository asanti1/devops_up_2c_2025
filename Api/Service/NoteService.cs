using Api.DAL;
using Api.DAL.Models;
using Api.DTO;
using Api.Mapping;
using Api.Mapping.Interface;
using Api.Service.Interface;

namespace Api.Service;

public class NoteService : INoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INoteMapper _mapper;
    public NoteService(IUnitOfWork unitOfWork, INoteMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NoteResponseDTO> AddNote(NoteAddRequestDTO noteAddRequest)
    {
        Note note = _mapper.ToEntity(noteAddRequest);
        Note noteResponse = await _unitOfWork.NoteRepository.AddNote(note);
        return _mapper.ToResponse(noteResponse);
    }

    public async Task<List<NoteResponseDTO>> GetAllNotes()
    {
        List<Note> notes = await _unitOfWork.NoteRepository.GetAllNotes();
        List<NoteResponseDTO> noteResponses = notes.Select(_mapper.ToResponse).ToList();
        return noteResponses;
    }

    public async Task<NoteResponseDTO> GetNoteById(int id)
    {
        Note? n = await _unitOfWork.NoteRepository.GetNoteById(id);
        if (n == null) throw new KeyNotFoundException();

        NoteResponseDTO noteResponse = _mapper.ToResponse(n);
        return noteResponse;
    }

    public async Task<NoteResponseDTO> Update(NoteUpdateRequestDTO n)
    {
        Note? note = await _unitOfWork.NoteRepository.Update(n);
        if (note == null) throw new KeyNotFoundException("No se encontró esa entidad de note.");

        return _mapper.ToResponse(note);
    }

    public async Task BorrarNote(int noteId)
    {
        if (!await _unitOfWork.NoteRepository.BorrarNote(noteId))
            throw new KeyNotFoundException("No se encontró esa entidad de note.");
    }

}