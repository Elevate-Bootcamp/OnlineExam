//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using OnlineExam.Domain;
//using OnlineExam.Features.Questions.Dtos;
//using System.Threading.Tasks;

//namespace OnlineExam.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class QuestionController : ControllerBase
//    {
//        private readonly IMediator _mediator;
//        public QuestionController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }
//        [HttpPost]
//        public async Task<IActionResult> AddQuestion(QuestionDTO questionDTO)
//        {
//            try
//            {
//                await _mediator.Send(new Features.Questions.Commands.CreateQuestionCommand(questionDTO));
//                return Ok();
//            } catch (Exception ex)
//            {
//                return BadRequest();
//            }
//        }
//        [HttpGet]
//        public async Task<List<QuestionDTO>> GetAllQuestions()
//        {

//            var Questions = await _mediator.Send(new Features.Questions.Queries.GetAllQuestionsQuery());
//            return Questions;

//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateQuestion(QuestionDTO questionDTO)
//        {
        
//            try
//            {
//                await _mediator.Send(new Features.Questions.Commands.UpdateQuestionCommand(questionDTO));
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteQuestion(int id)
//        {
//            try
//            {
//                await _mediator.Send(new Features.Questions.Commands.DeleteQuestionCommand(id));
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}
