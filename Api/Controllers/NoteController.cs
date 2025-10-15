using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private static List<string> _data = new List<string>() {
        "A", "B", "C"
    };
        [HttpGet]
        public ActionResult<List<string>> GetTasks()
        {
            return Ok(_data);
        }

        [HttpGet("GetTaskById")]
        public ActionResult<string> GetTaskById([FromQuery] int id)
        {
            string val = _data[id];
            return Ok(val);
        }

        [HttpPost]
        public ActionResult AddTask([FromBody] string value)
        {
            _data.Add(value);
            return Created();
        }

        [HttpPut]
        public ActionResult UpdateTask([FromBody] string id, string newValue)
        {
            return Ok();
        }

        [HttpDelete]
        public ActionResult DeleteTask([FromBody] string id)
        {
            _data.Remove(id);
            return NoContent();
        }
    }
}