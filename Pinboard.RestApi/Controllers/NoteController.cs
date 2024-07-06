using Microsoft.AspNetCore.Mvc;
using Pinboard.Domain.Interfaces.Inputs;
using Pinboard.Domain.Interfaces.UseCases;
using Pinboard.Domain.Model;
using System.Net;

namespace Pinboard.RestApi.Controllers
{
    [ApiController]
    [Route("api/notes")]
    public class NoteController : ControllerBase
    {
        private readonly INoteUseCases _noteUseCases;

        public NoteController(INoteUseCases noteUseCases)
        {
            _noteUseCases = noteUseCases;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Note>), (int)HttpStatusCode.OK)]
        public IActionResult Get()
        {
            var notes = _noteUseCases.GetNotes();
            return Ok(notes);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Note), (int)HttpStatusCode.Created)]
        public IActionResult Create([FromBody] NoteInput input)
        {
            var note = _noteUseCases.AddNote(input);
            return Created(note.Id, note);
        }

        [HttpPatch("{id}/title")]
        [ProducesResponseType(typeof(Note), (int)HttpStatusCode.Created)]
        public IActionResult UpdateTitle(string id, [FromBody] string title)
        {
            var note = _noteUseCases.UpdateTitle(id, title);
            return Ok(note);
        }

        [HttpPatch("{id}/content")]
        [ProducesResponseType(typeof(Note), (int)HttpStatusCode.Created)]
        public IActionResult UpdateContent(string id, [FromBody] string content)
        {
            var note = _noteUseCases.UpdateContent(id, content);
            return Ok(note);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Note), (int)HttpStatusCode.NoContent)]
        public IActionResult Create([FromBody] IEnumerable<string> ids)
        {
            _noteUseCases.DeleteNotes(ids);
            return NoContent();
        }
    }
}
