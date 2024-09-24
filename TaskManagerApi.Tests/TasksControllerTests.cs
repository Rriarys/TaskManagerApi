using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Controllers;
using TaskManagerApi.Data;


namespace TaskManagerApi.Tests
{
    public class TasksControllerTests
    {

        private readonly AppDbContext _context;
        private readonly TasksController _controller;

        public TasksControllerTests() 
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            _context = new AppDbContext(options);

            //celan db before every test
            _context.Tasks.RemoveRange(_context.Tasks);
            _context.SaveChanges();

            _controller = new TasksController(_context);

            //add test data
            _context.Tasks.AddRange(
                new TaskItem { Title  = "Test Task 1", Description = "Description 1", Status = "� ��������" },
                new TaskItem { Title = "Test Task 2", Description = "Description 2", Status = "���������" }
                );
            _context.SaveChanges();
        }

        //GET all
        [Fact]
        public async Task GetTasks_ReturnsAllTasks()
        {
            var result = await _controller.GetTasks();
            var actionResult = Assert.IsType<ActionResult<IEnumerable<TaskItem>>>(result);
            var tasks = Assert.IsAssignableFrom<IEnumerable<TaskItem>>(actionResult.Value);

            Assert.Equal(2, tasks.Count());
        }

        //Get by ID
        [Fact]
        public async Task GetTasks_ReturnsTaskById()
        {
            var result = await _controller.GetTask(1);
            var actionResult = Assert.IsType<ActionResult<TaskItem>>(result);
            var task = Assert.IsType<TaskItem>(actionResult.Value);

            Assert.Equal("Test Task 1", task.Title);
        }

        //POST
        [Fact]
        public async Task PostTask_CreatesNewTask()
        {
            // ��������� ������
            var newTask = new TaskItem { Title = "New Task", Description = "New Description", Status = "� ��������" };
            
            var result = await _controller.PostTask(newTask);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var task = Assert.IsType<TaskItem>(createdAtActionResult.Value);

            Assert.Equal("New Task", task.Title);
        }

        //PUT
        [Fact]
        public async Task PutTask_UpdatesExistingTasks()
        {
            // ��������� ������, ����� ����� �� ��������
            var taskToUpdate = new TaskItem { Title = "Original Task", Description = "Original Description", Status = "� ��������" };
            _context.Tasks.Add(taskToUpdate);
            await _context.SaveChangesAsync();

            // ������ ������ ������
            taskToUpdate.Title = "Updated Task";
            taskToUpdate.Description = "Updated Description";
            taskToUpdate.Status = "���������";

            // ��������� ������
            var result = await _controller.PutTask(taskToUpdate.Id, taskToUpdate);

            // ���������, ��� ��������� - NoContent
            Assert.IsType<NoContentResult>(result);
        }

        //DELETE
        [Fact]
        public async Task DeleteTask_RemoveTask()
        {
            // ��������� ������ � ���� ������ ��� ������������ ��������
            var taskToDelete = new TaskItem { Title = "Task to Delete", Description = "Description", Status = "� ��������" };
            _context.Tasks.Add(taskToDelete);
            await _context.SaveChangesAsync();

            // ������� ������
            var result = await _controller.DeletTask(taskToDelete.Id);

            // ���������, ��� ��������� - NoContent
            Assert.IsType<NoContentResult>(result);
        }
    }
}