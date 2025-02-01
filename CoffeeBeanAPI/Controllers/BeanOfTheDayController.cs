using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeBeanAPI.Data;
using CoffeeBeanAPI.Models;

namespace CoffeeBeanAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BeanOfTheDayController : ControllerBase
    {
        private readonly AppDbContext _context;
        private static Random _random = new Random();

        public BeanOfTheDayController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Bean>> GetBeanOfTheDay()
        {
            var today = DateTime.UtcNow.Date;
            var existingBean = await _context.BeanOfTheDays
                .Include(b => b.Bean)
                .FirstOrDefaultAsync(b => b.SelectedDate == today);

            if (existingBean != null) return existingBean.Bean;

            // Reset previous Bean of the Day
            var previousBeanOfTheDay = await _context.Beans
                .FirstOrDefaultAsync(b => b.isBOTD);
            if (previousBeanOfTheDay != null)
            {
                previousBeanOfTheDay.isBOTD = false;
            }

            var allBeans = await _context.Beans.ToListAsync();
            var previousBean = await _context.BeanOfTheDays
                .OrderByDescending(b => b.SelectedDate)
                .Select(b => b.BeanId)
                .FirstOrDefaultAsync();

            var availableBeans = allBeans.Where(b => b.Id != previousBean).ToList();
            if (!availableBeans.Any()) return NotFound("No available beans.");

            var selectedBean = availableBeans[_random.Next(availableBeans.Count)];
            selectedBean.isBOTD = true;

            var newBOTD = new BeanOfTheDay
            {
                BeanId = selectedBean.Id,
                SelectedDate = today
            };

            _context.BeanOfTheDays.Add(newBOTD);
            await _context.SaveChangesAsync();

            return selectedBean;
        }
    }

}
