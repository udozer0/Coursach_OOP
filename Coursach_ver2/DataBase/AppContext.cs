using Coursach_ver2.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursach_ver2.DataBase
{
    /// <summary>
    /// Контекст базы данных для приложения, использующего Entity Framework Core и SQLite.
    /// Содержит DbSet для моделей Student, Class, Grade и Subject.
    /// </summary>
    public class AppContext : DbContext
    {
        /// <summary>
        /// Таблица студентов.
        /// </summary>
        public DbSet<Student> Students { get; set; } = null!;

        /// <summary>
        /// Таблица классов.
        /// </summary>
        public DbSet<Class> Classes { get; set; } = null!;

        /// <summary>
        /// Таблица оценок.
        /// </summary>
        public DbSet<Grade> Grades { get; set; } = null!;

        /// <summary>
        /// Таблица предметов.
        /// </summary>
        public DbSet<Subject> Subjects { get; set; } = null!;

        private string _dbPath;

        /// <summary>
        /// Конструктор, инициализирующий путь к базе данных.
        /// </summary>
        /// <param name="dbPath">Путь к базе данных</param>
        public AppContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        /// <summary>
        /// Конструктор по умолчанию, инициализирующий базу данных с именем School.db.
        /// </summary>
        public AppContext()
        {
            _dbPath = "School.db";
        }

        /// <summary>
        /// Конфигурирование параметров подключения к базе данных SQLite.
        /// </summary>
        /// <param name="optionsBuilder">Построитель параметров опций</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }

        /// <summary>
        /// Асинхронное создание базы данных.
        /// </summary>
        /// <returns>Задача создания базы данных</returns>
        public async Task CreateDatabaseAsync()
        {
            await Database.EnsureCreatedAsync();
        }

        /// <summary>
        /// Асинхронное удаление базы данных.
        /// </summary>
        /// <returns>Задача удаления базы данных</returns>
        public async Task DeleteDatabaseAsync()
        {
            await Database.EnsureDeletedAsync();
        }

        /// <summary>
        /// Проверяет, существует ли база данных.
        /// </summary>
        /// <returns>True, если база данных существует, иначе False</returns>
        public bool DatabaseExists()
        {
            return File.Exists(_dbPath);
        }

        /// <summary>
        /// Сохраняет копию базы данных в указанном пути.
        /// </summary>
        /// <param name="filePath">Путь к файлу для сохранения базы данных</param>
        public void SaveDatabase(string filePath)
        {
            if (File.Exists(_dbPath))
            {
                File.Copy(_dbPath, filePath, true);
            }
            else
            {
                throw new FileNotFoundException("Database file not found.");
            }
        }

        /// <summary>
        /// Конфигурирование моделей для построения связей между сущностями базы данных.
        /// </summary>
        /// <param name="modelBuilder">Построитель моделей</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassId);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId);

            modelBuilder.Entity<Grade>()
               .HasOne(g => g.Subject)
               .WithMany(sub => sub.Grades)
               .HasForeignKey(g => g.SubjectId);
        }
    }
}
