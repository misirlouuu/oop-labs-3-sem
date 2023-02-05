using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups = new ();
        private readonly List<Student> _students = new ();

        public Group AddGroup(GroupName name)
        {
            ArgumentNullException.ThrowIfNull(name);
            if (_groups.Any(group => group.GroupName.Equals(name)))
            {
                throw new IsuException($"group {name} already contains in group list");
            }

            var group = new Group(name);
            _groups.Add(group);

            return group;
        }

        public Student AddStudent(Group group, string firstName, string lastName)
        {
            ArgumentNullException.ThrowIfNull(group);
            var student = new Student(firstName, lastName, group);
            if (_students.Contains(student))
            {
                throw new IsuException($"{student} already contains in _students");
            }

            _students.Add(student);
            group.AddStudent(student);

            return student;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (!_groups.Contains(newGroup))
            {
                throw new IsuException($"{newGroup} wasn't added in _groups");
            }

            if (!_students.Contains(student))
            {
                throw new IsuException($"{student} wasn't added in _students");
            }

            student.ChangeGroup(newGroup);
        }

        public Group FindGroup(GroupName groupName)
        {
            return _groups.Single(x => x.GroupName == groupName);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.Where(x => x.GroupName.Course == courseNumber).ToList();
        }

        public Student FindStudent(int id)
        {
            return _students.Single(x => x.Id == id);
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return _students.Where(x => x.Group.GroupName == groupName).ToList();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return _students.Where(x => x.Group.GroupName.Course == courseNumber).ToList();
        }

        public Student GetStudent(int id)
        {
            return _students.Single(x => x.Id == id);
        }
    }
}