//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using StudentTeacherApp.DTOs;
//using StudentTeacherApp.Services;

//namespace StudentTeacherApp.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize]
//    public class RecordsController : ControllerBase
//    {
//        private readonly IRecordService _recordService;

//        public RecordsController(IRecordService recordService)
//        {
//            _recordService = recordService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var records = await _recordService.GetAllRecords();
//            return Ok(records);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var record = await _recordService.GetRecordById(id);
//            if (record == null) return NotFound();
//            return Ok(record);
//        }

//        [HttpPost]
//        [Authorize(Roles = "Teacher")]
//        public async Task<IActionResult> Create([FromBody] RecordDto dto)
//        {
//            var record = await _recordService.CreateRecord(dto);
//            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
//        }

//        [HttpPut("{id}")]
//        [Authorize(Roles = "Teacher")]
//        public async Task<IActionResult> Update(int id, [FromBody] RecordDto dto)
//        {
//            var record = await _recordService.UpdateRecord(id, dto);
//            if (record == null) return NotFound();
//            return Ok(record);
//        }

//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Teacher")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await _recordService.DeleteRecord(id);
//            if (!result) return NotFound();
//            return NoContent();
//        }
//    }
//}
