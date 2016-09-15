using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;

namespace SchoolManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("enter a number");
            //var num = Convert.ToInt32(Console.ReadLine());
            //CheckId(num);

            //Ask operations
            Console.WriteLine("Hi Manager! Plz take the option that you wanted to operate.\n \n 1.Create new Instructor and assign instructor to the Course \n \n 2.Create new Student and enroll to the Course \n \n 3.Create new Course and assign to Department \n \n 4.View all Student grades of Course \n \n 5.View all Course and Instructors ");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Console.WriteLine("You have chosen to create new Instructor and assign instructor to the Course");
                    CreateNewInstructorAndAssign();
                    break;
                case "2":
                    Console.WriteLine("You have chosen to create new Student and enroll to the Course");
                    CreateNewStuAndEnroll();
                    break;
                case "3":
                    Console.WriteLine("You have chosen to create new course and assign to department");
                    CreateNewCourseAndAssignToDep();
                    //ViewCourse();
                    break;
                case "4":
                    ViewStuGrade();
                    break;
                case "5": 
                    ViewCourseAndInstructor();//View all Course and Instructors
                    break;
                default:
                    break;
            }
            Console.ReadKey();
            
        }
        public static void CreateNewInstructorAndAssign()
        {
            using(var db = new SchoolEntities()){

            //var instructor = new CourseInstructor();

            Console.WriteLine("Plz input the first name of the instructor");
            var inputFirst = Console.ReadLine();

            Console.WriteLine("Plz input the last name of the instructor");
            var inputLast = Console.ReadLine();

            Console.WriteLine("Plz enter the hire date");
            var inputHireDate = Convert.ToDateTime(Console.ReadLine());

            var instructor = new Person()
            {
                FirstName = inputFirst,
                LastName = inputLast,
                HireDate = inputHireDate

            };
            db.People.Add(instructor);//Save new people info
            db.SaveChanges();

            //ASSIGN INSTRUCTOR in courses
            Console.WriteLine("Plz select the course name to assign the instructor in");
            ViewCourse();
            var courseName = Console.ReadLine();//Course title

            var courseIdList = db.Courses.Where(x => x.Title == courseName).Select(y => y.CourseID).ToList();
            var personIdList = db.People.Where(x => x.FirstName == inputFirst).Select(y => y.PersonID).ToList();
               
            foreach (var item in personIdList)
            {
                Console.WriteLine(item);
            }

                //Enter the info of the INSTRUCTOR in Office Assignment
                Console.WriteLine("Plz enter the location of office assignment");
                var location = Console.ReadLine();

                OfficeAssignment oa = new OfficeAssignment()
                {
                    InstructorID = personIdList[0],//The same as the person id which has been created
                    Location = location,
                };

                db.OfficeAssignments.Add(oa);
                db.SaveChanges();

            CourseInstructor c = new CourseInstructor()
            {
                CourseID = courseIdList[0],
                PersonID = personIdList[0] 
            };

            

            
            db.CourseInstructors.Add(c);
            db.SaveChanges();
            //using (var db = new SchoolEntities())
            //{
            //    foreach (var item in db.Courses)
            //    {
                    
            //    }
            //    db.People.Add(person);
            //    db.SaveChanges();
            //}
            
            }
        }

        public static void CreateNewStuAndEnroll()
        {
            using(var db = new SchoolEntities()){
                //Enter student information
                Console.WriteLine("Plz enter the first name of the student");
                var stuFirst = Console.ReadLine();

                Console.WriteLine("Plz enter the last name of the student");
                var stuLast = Console.ReadLine();

                Console.WriteLine("Plz enter the enrollment date of the student");
                var stuEnrollDate = Convert.ToDateTime( Console.ReadLine());

                var student = new Person()
                {
                    FirstName = stuFirst,
                    LastName = stuLast,
                    EnrollmentDate = stuEnrollDate
                };
                //Save student changes
                db.People.Add(student);
                db.SaveChanges();

                //Enroll the student to course
                Console.WriteLine("Plz select the course name to enroll the student in");
                ViewCourse();
                var courseName = Console.ReadLine();//Course title

                //input the grade of the student
                Console.WriteLine("Plz enter the grade to the student");
                var grade = Convert.ToDouble(Console.ReadLine());

                var idListCourse = db.Courses.Where(x => x.Title == courseName).Select(y => y.CourseID).ToList();
                var idStuList = db.People.Where(x => x.FirstName == stuFirst).Select(y => y.PersonID).ToList();

                var sg = new StudentGrade()
                {
                    CourseID = idListCourse[0],
                    StudentID = idStuList[0],
                    Grade = grade
                };

                db.StudentGrades.Add(sg);
                db.SaveChanges();
            }
        }

        public static void CreateNewCourseAndAssignToDep()
        {
            using(var db = new SchoolEntities()){
                Console.WriteLine("Enter the name of the new course");
                var courseName = Console.ReadLine();//Title

                Console.WriteLine("Enter the credits of the course");
                var credit = Convert.ToInt32( Console.ReadLine());

                //Assign the course to the department 
                Console.WriteLine("Plz select the department ID that you want to assign the course in");
                ViewDep();
                var depId = Convert.ToInt32(Console.ReadLine());

                //Check if the id exits

                var course = new Course()
                {
                    Title = courseName,
                    Credits = credit,
                    DepartmentID = depId
                };

                db.Courses.Add(course);
                db.SaveChanges();
                
            }
        }

        public static void ViewCourse()
        {
            using(var db = new SchoolEntities()){
                foreach (var item in db.Courses)
                {
                    Console.WriteLine("Course ID: {0}  Course Name: {1} \n", item.CourseID, item.Title);
                }
            }
            
        }

        public static void ViewDep()
        {
             using(var db = new SchoolEntities()){
                 foreach (var item in db.Departments)
                 {
                     Console.WriteLine("Department ID: {0} Department Name: {1} \n", item.DepartmentID, item.Name);
                 }
             }
        }

        public static void CheckId(int id)
        {
            using(var db = new SchoolEntities()){
                 var flag = db.Departments.Select(x => x.DepartmentID == id);
                 
            }
        }

        public static void ViewStuGrade()
        {
            Console.Clear();
            using(var db = new SchoolEntities()){
                var StuGrade = db.ViewStudentGrade().ToList();

                
                               
                foreach (var item in StuGrade)
                {
                    Console.WriteLine("Student ID: {0}    Name: {1}    Enrolled course: {2} \n", item.PersonID, item.FirstName + " " + item.LastName, item.Grade);
                }
            }
        }

        public static void ViewCourseAndInstructor()
        {
            //SqlConnection conn = new SqlConnection("Server=SAM\SQLEXPRESS")
            //SqlDataReader sdr = null;
            //SqlCommand cmd = new SqlCommand("dbo.ViewCourseAndInstructor", conn);
            Console.Clear();
            using(var db = new SchoolEntities()){
                var Result = db.ViewCourseAndInstructor().ToList();
                foreach (var item in Result)
                {
                    Console.WriteLine("Instructor ID: {0}    Name: {1}    Assigned course: {2} \n", item.PersonID, item.FirstName + " " + item.LastName, item.Title);
                }
            }
            
        }
        
    }
}
