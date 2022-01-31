﻿using StudentDiary.Models.Domains;
using StudentDiary.Models.Wrappers;
using System.Collections.Generic;
using System.Linq;

namespace StudentDiary.Models.Converters
{
    public static class StudentConverter
    {
        public static StudentWrapper ToWrapper(this Student model)
        {
            return new StudentWrapper
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Activities = model.Activities,
                Comments = model.Comments,
                Group = new GroupWrapper { Id = model.Group.Id, Name = model.Group.Name },
                Math = string.Join(",", model.Ratings.Where(x => x.SubjectId == (int)Subject.Math).Select(y => y.Rate)),
                Physics = string.Join(",", model.Ratings.Where(x => x.SubjectId == (int)Subject.Physics).Select(y => y.Rate)),
                PolishLang = string.Join(",", model.Ratings.Where(x => x.SubjectId == (int)Subject.PolishLang).Select(y => y.Rate)),
                ForeignLang = string.Join(",", model.Ratings.Where(x => x.SubjectId == (int)Subject.ForeignLang).Select(y => y.Rate)),
                Technology = string.Join(",", model.Ratings.Where(x => x.SubjectId == (int)Subject.Technology).Select(y => y.Rate))
            };
        }

        public static Student toDao(this StudentWrapper model)
        {
            return new Student
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Activities = model.Activities,
                Comments = model.Comments,
                GroupId = model.Group.Id
            };
        }
        public static List<Rating> ToRatingDao(this StudentWrapper model)
        {
            var ratings = new List<Rating>();
            if (!string.IsNullOrWhiteSpace(model.Math))
            {
                model.Math.Split(',').ToList().ForEach(x => ratings.Add(new Rating 
                {
                Rate=int.Parse(x),
                 StudentId=model.Id,
                 SubjectId=(int)Subject.Math                
                }));
            }

            if (!string.IsNullOrWhiteSpace(model.Technology))
            {
                model.Technology.Split(',').ToList().ForEach(x => ratings.Add(new Rating
                {
                    Rate = int.Parse(x),
                    StudentId = model.Id,
                    SubjectId = (int)Subject.Technology
                }));
            }
            if (!string.IsNullOrWhiteSpace(model.Physics))
            {
                model.Physics.Split(',').ToList().ForEach(x => ratings.Add(new Rating
                {
                    Rate = int.Parse(x),
                    StudentId = model.Id,
                    SubjectId = (int)Subject.Physics
                }));
            }
            if (!string.IsNullOrWhiteSpace(model.PolishLang))
            {
                model.PolishLang.Split(',').ToList().ForEach(x => ratings.Add(new Rating
                {
                    Rate = int.Parse(x),
                    StudentId = model.Id,
                    SubjectId = (int)Subject.PolishLang
                }));
            }
            if (!string.IsNullOrWhiteSpace(model.ForeignLang))
            {
                model.ForeignLang.Split(',').ToList().ForEach(x => ratings.Add(new Rating
                {
                    Rate = int.Parse(x),
                    StudentId = model.Id,
                    SubjectId = (int)Subject.ForeignLang
                }));
            }


            return ratings;
        }
    }
}
