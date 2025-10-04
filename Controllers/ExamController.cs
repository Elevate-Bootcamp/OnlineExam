using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.Features.Exams.Commands;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Features.Exams.Queries;

namespace OnlineExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        public ExamController(IMediator mediator,IWebHostEnvironment env)
        {
            _mediator = mediator;
            _env = env;
        }      

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(object),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateExamFormDto form,CancellationToken ct)
        {
            if(form.Icon is null || form.Icon.Length == 0)
            {
                ModelState.AddModelError(nameof(form.Icon),"Icon image is required.");
                return ValidationProblem(ModelState);
            }

            // Save icon file to wwwroot/uploads
            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(),"wwwroot");
            var uploads = Path.Combine(webRoot,"uploads");
            Directory.CreateDirectory(uploads);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(form.Icon.FileName)}";
            var filePath = Path.Combine(uploads,fileName);
            await using(var fs = System.IO.File.Create(filePath))
                await form.Icon.CopyToAsync(fs,ct);

            var iconUrl = $"/uploads/{fileName}";

            var cmd = new CreateExamCommand(
                Title: form.Title,
                IconUrl: iconUrl,
                CategoryId: form.CategoryId,
                StartDate: form.StartDate,   // local as requested
                EndDate: form.EndDate,
                DurationMinutes: form.Duration,
                Description: form.Description
            );

            var id = await _mediator.Send(cmd,ct);            
            return Created();
           // return CreatedAtAction(nameof(GetById),new { id },new { id });---> todo
        }



        //// GET: api/exams/{id}  (simple stub to make CreatedAtAction valid)
        //[HttpGet("{id:int}")]
        //[ProducesResponseType(typeof(ExamDto),StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetById([FromRoute] int id,CancellationToken ct)
        //{
        //    var dto = await _mediator.Send(new GetExamByIdQuery(id),ct);
        //    return dto is null ? NotFound() : Ok(dto);
        //}


    }
}
