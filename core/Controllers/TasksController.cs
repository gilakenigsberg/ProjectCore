using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Controllers{

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "Agent")]

public class TasksController : ControllerBase
{
    private long userId;
    private ITaskService TaskService;

    public TasksController(ITaskService TaskService)
    {
        this.TaskService= TaskService;
        this.userId = long.Parse(User.FindFirst("UserId")?.Value ?? "");
    }

    [HttpGet]
   public ActionResult<List<Task>> GetAll() =>
        TaskService.GetAll(userId);


    [HttpGet("{id}")]
    public ActionResult<Task> GetById(int id)
        {
            var task = TaskService.GetById(userId, id);
            if (task == null)
                return NotFound();
            return task;
        }

    [HttpPost]
    public IActionResult Create(Task task)
        {
            TaskService.Add(userId, task);
            return CreatedAtAction(nameof(Create), new {id=task.Id}, task);
        }

    [HttpPut("{id}")]
   public IActionResult Update(int id, Task task)
        {
            if (id != task.Id)
                return BadRequest();
            var myTask = TaskService.GetById(userId, id);
            if (myTask is null)
                return  NotFound();
            TaskService.Update(userId, task);
            return NoContent();
        }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
        {
            var task = TaskService.GetById(userId, id);
            if (task is null)
                return  NotFound();

            TaskService.Delete(userId, id);

            return Content(TaskService.Count(userId).ToString());
        }

   } 
    
}

