using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoList.Domain.Entities;
using TodoList.Domain;
using TodoList.Dto.Notes;
using TodoList.Infra.Data.Repository.Interface;

namespace TodoList.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository _notesRepository;
        private readonly IMapper _mapper;

        public NotesController(INotesRepository notesRepository, IMapper mapper)
        {
            _notesRepository = notesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]        
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Notes>>> GetAllNotes()
        {
            var notes = await _notesRepository.GetAsync();

            if (notes.Any()) 
                return Ok(new { status = StatusResponse.Success, data = notes });

            return NotFound(new { status = StatusResponse.Failed, message = MessageResponse.NoData });
        }

        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Notes>> GetNoteById(int id)
        {
            var note = await _notesRepository.GetByIdAsync(id);

            if (note is null) 
                return NotFound(new { status = StatusResponse.Failed, message = MessageResponse.NoDataById });

            return note;
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<ActionResult<Notes>> CreateNote([FromBody] NotesAddOrUpdateDto model)
        {
            return Ok(new { data = await _notesRepository.CreateOrEdit(_mapper.Map<Notes>(model)) });
        }

        [HttpPut("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Notes>> UpdateNote(int id, [FromBody] NotesAddOrUpdateDto model)
        {
            var singleNote = await _notesRepository.GetByIdAsync(id);

            if (singleNote is null) 
                return NotFound(new { status = StatusResponse.Failed, message = MessageResponse.NoDataById });

            singleNote.Title = model.Title;
            singleNote.Content = model.Content;

            return Ok(new {status = StatusResponse.Success, data = await _notesRepository.CreateOrEdit(singleNote) });
        }


        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteNote(int id)
        {
            var note = await _notesRepository.GetByIdAsync(id);

            if (note is not null)
            {
                await _notesRepository.Delete(note);
                return Ok(new { status = StatusResponse.Success});
            }

            return NotFound(new { status = StatusResponse.Failed, message = MessageResponse.NoDataById });
        }
    }
}
