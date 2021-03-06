using StudentDiary.Models.Domains;
using StudentDiary.Models.Wrappers;
using StudentDiary.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using StudentDiary.Models;
using StudentDiary.Properties;
using System.Data;
using System.Data.SqlClient;

namespace StudentDiary
{
    public class Repository
    {
        public List<Group> GetGroups()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Groups.ToList();
            }
        }

        public List<StudentWrapper> GetStudents(int groupId)
        {
            using (var context = new ApplicationDbContext())
            {
                var students = context.Students
                    .Include(x => x.Group)
                    .Include(y => y.Ratings)
                    .AsQueryable();

                if (groupId != 0)
                {
                    students = students.Where(x => x.GroupId == groupId);
                }

                return students
                    .ToList()
                    .Select(x => x
                    .ToWrapper())
                    .ToList();
            }
        }

        public void DeleteStudent(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var studentToDelete = context.Students.Find(id);
                context.Students.Remove(studentToDelete);
                context.SaveChanges();
            }
        }

        public void UpdateStudent(StudentWrapper studentWrapper)
        {
            var student = studentWrapper.toDao();
            var ratings = studentWrapper.ToRatingDao();

            using (var context = new ApplicationDbContext())
            {
                UpdateStudentProperties(context, student);

                var studentRatings = context.Ratings.Where(x => x.StudentId == student.Id).ToList();

                UpdateRate(student, ratings, context, studentRatings, Subject.Math);
                UpdateRate(student, ratings, context, studentRatings, Subject.Technology);
                UpdateRate(student, ratings, context, studentRatings, Subject.Physics);
                UpdateRate(student, ratings, context, studentRatings, Subject.PolishLang);
                UpdateRate(student, ratings, context, studentRatings, Subject.ForeignLang);
                context.SaveChanges();
            }
        }

        private void UpdateStudentProperties(ApplicationDbContext context, Student student)
        {
            var studentToUpdate = context.Students.Find(student.Id);
            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.LastName = student.LastName;
            studentToUpdate.Activities = student.Activities;
            studentToUpdate.Comments = student.Comments;
            studentToUpdate.GroupId = student.GroupId;
        }

        private static void UpdateRate(Student student, List<Rating> ratings, ApplicationDbContext context, List<Rating> studentRatings, Subject subject)
        {
            var subjectRatings = studentRatings.Where(x => x.SubjectId == (int)subject).Select(x => x.Rate);
            var newsubjectRatings = ratings.Where(x => x.SubjectId == (int)subject).Select(x => x.Rate);

            var subjectRatingsToDelete = subjectRatings.Except(newsubjectRatings).ToList();
            var subjectRatingsToAdd = newsubjectRatings.Except(subjectRatings).ToList();

            subjectRatingsToDelete.ForEach(x =>
            {
                var ratingToDelete = context.Ratings.First(
                    y => y.Rate == x &&
                    y.StudentId == student.Id &&
                    y.SubjectId == (int)subject);
                context.Ratings.Remove(ratingToDelete);
            });

            subjectRatingsToAdd.ForEach(x =>
            {
                var ratingToAdd = new Rating
                {
                    Rate = x,
                    StudentId = student.Id,
                    SubjectId = (int)subject
                };
                context.Ratings.Add(ratingToAdd);
            });
        }

        public void AddStudent(StudentWrapper studentWrapper)
        {
            var student = studentWrapper.toDao();
            var ratings = studentWrapper.ToRatingDao();

            using (var context = new ApplicationDbContext())
            {
                var dbStudent = context.Students.Add(student);
                ratings.ForEach(x =>
                {
                    x.Id = dbStudent.Id;
                    context.Ratings.Add(x);
                });
                context.SaveChanges();
            }
        }
    }
}
