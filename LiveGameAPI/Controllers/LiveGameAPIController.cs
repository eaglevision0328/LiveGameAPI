using LiveGameAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiveGameAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LiveGameAPIController : ControllerBase
    {       
        private static GameState _currentState = new GameState();                

        [HttpPost("InitState")]
        public IActionResult InitState([FromBody] InitStateRequest request)
        {
            //initialize grid board state 
            _currentState.initBoard(request.Board, request.Rows, request.Columns);
            return Ok(new { message = "Init grid state", grid_id = _currentState.CurrentTurn});
        }


        [HttpPost("NextState")]
        public IActionResult NextState()
        {
            //calculate grid board's next state 
            _currentState.nextGrid();

            //if occured error
            if (_currentState.isError)
            {
                return BadRequest(new { message = "error", grid_id = _currentState.CurrentTurn});
            }
            return Ok(new { message = "Next state", grid_id = _currentState.CurrentTurn, state = _currentState.convertState()});
        }

        [HttpPost("NextStateOfXNumbers")]
        public IActionResult NextStateOfXNumbers([FromBody] int x)
        {
            //calculate grid board's x number of states
            _currentState.nextGrid(x);

            //if occured error
            if (_currentState.isError)
            {
                return BadRequest(new { message = "error" });
            }
            return Ok(new { message = $"{x} number of states", grid_id = _currentState.CurrentTurn, states = _currentState.convertState() });
        }

        [HttpPost("FinalState")]
        public IActionResult FinalState()
        {           
            return Ok(new { message = "Final state ", grid_id = _currentState.CurrentTurn, state = _currentState.convertState() });
        }

        //parameter struct for api input
        public class InitStateRequest
        {
            public int Rows { get; set; }
            public int Columns { get; set; }
            public int[] Board { get; set; }
        }

    }

}
