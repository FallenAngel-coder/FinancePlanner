using System;
using System.Collections.Generic;
using System.Linq;
using FinancePlanner.Repositories;

namespace FinancePlanner.Services
{
    public class CategoryProjection
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Projected { get; set; }
        public decimal Actual { get; set; }
    }

    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepo;
        private readonly TransactionRepository _transactionRepo;

        public CategoryService(CategoryRepository categoryRepo, TransactionRepository transactionRepo)
        {
            _categoryRepo = categoryRepo;
            _transactionRepo = transactionRepo;
        }

        public bool IsCategoryActive(Category c, int year, int month)
        {
            if (c.StartYear > 0)
            {
                if (year < c.StartYear || (year == c.StartYear && month < c.StartMonth))
                    return false;
            }

            if (c.EndYear > 0)
            {
                if (year > c.EndYear || (year == c.EndYear && month > c.EndMonth))
                    return false;
            }

            return true;
        }

        public bool IsCategoryActiveInRange(Category c, DateTime start, DateTime end)
        {
            // Перевірка активності категорії у діапазоні дат [start, end]
            
            // Дата початку категорії
            DateTime catStart = DateTime.MinValue;
            if (c.StartYear > 0)
            {
                catStart = new DateTime(c.StartYear, c.StartMonth, 1);
            }
                
            // Дата завершення категорії
            DateTime catEnd = DateTime.MaxValue;
            if (c.EndYear > 0)
            {
                // Остання секунда останнього дня місяця
                catEnd = new DateTime(c.EndYear, c.EndMonth, 1).AddMonths(1).AddSeconds(-1);
            }
                
            // Перетин діапазонів: [catStart, catEnd] та [start, end]
            return catStart <= end && catEnd >= start;
        }

        public List<CategoryProjection> GetProjectionsForMonth(int year, int month)
        {
            var categories = _categoryRepo.GetAll()
                .Where(c => IsCategoryActive(c, year, month))
                .ToList();

            var allTransactions = _transactionRepo.GetAll();

            var monthTransactions = allTransactions
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .ToList();

            var projections = new List<CategoryProjection>();

            foreach (var category in categories)
            {
                decimal actual = monthTransactions
                    .Where(t => t.CategoryId == category.Id && t.Type == category.Type)
                    .Sum(t => t.Amount);

                projections.Add(new CategoryProjection
                {
                    CategoryId = category.Id,
                    Name = category.Name,
                    Type = category.Type,
                    Projected = category.ProjectedAmount,
                    Actual = actual
                });
            }

            return projections;
        }

        public void UpdateProjection(int categoryId, decimal newProjection)
        {
            var category = _categoryRepo.GetAll().FirstOrDefault(c => c.Id == categoryId);
            if (category != null)
            {
                category.ProjectedAmount = newProjection;
                _categoryRepo.Update(category);
            }
        }

        public void DeleteCategory(int categoryId, int scope, int currentYear, int currentMonth, int? reassignToId = null)
        {
            var category = _categoryRepo.GetAll().FirstOrDefault(c => c.Id == categoryId);
            if (category == null) return;

            if (scope == 0) // Всі
            {
                _transactionRepo.ReassignTransactions(categoryId, reassignToId);
                _categoryRepo.Delete(categoryId);
            }
            else if (scope == 1) // Тільки для цього місяця
            {
                _transactionRepo.ReassignTransactions(categoryId, reassignToId, currentYear, currentMonth);

                int endMonth1 = currentMonth - 1;
                int endYear1 = currentYear;
                if (endMonth1 < 1) { endMonth1 = 12; endYear1--; }

                int startMonth2 = currentMonth + 1;
                int startYear2 = currentYear;
                if (startMonth2 > 12) { startMonth2 = 1; startYear2++; }

                // Створюємо продовження категорії, якщо вона не закінчується раніше
                if (category.EndYear == 0 || (category.EndYear > startYear2) || (category.EndYear == startYear2 && category.EndMonth >= startMonth2))
                {
                    var catB = new Category { Name = category.Name, Type = category.Type, ProjectedAmount = category.ProjectedAmount, StartMonth = startMonth2, StartYear = startYear2, EndMonth = category.EndMonth, EndYear = category.EndYear };
                    _categoryRepo.Add(catB);
                }

                category.EndMonth = endMonth1;
                category.EndYear = endYear1;
                _categoryRepo.Update(category);
            }
            else if (scope == 2) // З цього місяця і далі
            {
                _transactionRepo.ReassignTransactions(categoryId, reassignToId, currentYear, currentMonth, true);

                int endMonth1 = currentMonth - 1;
                int endYear1 = currentYear;
                if (endMonth1 < 1) { endMonth1 = 12; endYear1--; }

                category.EndMonth = endMonth1;
                category.EndYear = endYear1;
                _categoryRepo.Update(category);
            }
        }

        public void RenameCategory(int categoryId, string newName, int scope, int currentYear, int currentMonth)
        {
            var category = _categoryRepo.GetAll().FirstOrDefault(c => c.Id == categoryId);
            if (category == null) return;

            if (scope == 0) // Всі
            {
                category.Name = newName;
                _categoryRepo.Update(category);
            }
            else if (scope == 1) // Тільки для цього місяця
            {
                // Створюємо нову категорію на 1 місяць
                var tempCat = new Category { Name = newName, Type = category.Type, ProjectedAmount = category.ProjectedAmount, StartMonth = currentMonth, StartYear = currentYear, EndMonth = currentMonth, EndYear = currentYear };
                _categoryRepo.Add(tempCat);
                
                var newCat = _categoryRepo.GetAll().OrderByDescending(c => c.Id).FirstOrDefault(c => c.Name == newName);
                int? newId = newCat?.Id;

                // Виключаємо цей місяць з оригінальної категорії та переносимо транзакції
                DeleteCategory(categoryId, 1, currentYear, currentMonth, newId);
            }
            else if (scope == 2) // З цього місяця і далі
            {
                // Створюємо нову категорію
                var newCat = new Category { Name = newName, Type = category.Type, ProjectedAmount = category.ProjectedAmount, StartMonth = currentMonth, StartYear = currentYear, EndMonth = category.EndMonth, EndYear = category.EndYear };
                _categoryRepo.Add(newCat);

                var newCatCreated = _categoryRepo.GetAll().OrderByDescending(c => c.Id).FirstOrDefault(c => c.Name == newName);
                int? newId = newCatCreated?.Id;

                // Завершуємо стару та переносимо транзакції
                DeleteCategory(categoryId, 2, currentYear, currentMonth, newId);
            }
        }
    }
}
