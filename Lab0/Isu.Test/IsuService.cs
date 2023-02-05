using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    private IsuService _isuService = new IsuService();

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var group = _isuService.AddGroup(new GroupName("M32131"));
        var student = _isuService.AddStudent(group, "Ivan", "Ivanov");
        Assert.Equal(student.Group, group);
        Assert.Contains(student, group.Students);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var group = _isuService.AddGroup(new GroupName("M32131"));
        for (int iStudent = 0; iStudent < 40; iStudent++)
        {
            _isuService.AddStudent(group, "Ivan", $"{iStudent}");
        }

        Assert.Throws<ReachMaxStudentPerGroupException>(() => _isuService.AddStudent(group, "Ivan", "41"));
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        Assert.Throws<InvalidGroupNameException>(() => _isuService.AddGroup(new GroupName("32131")));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var group1 = _isuService.AddGroup(new GroupName("M11111"));
        var group2 = _isuService.AddGroup(new GroupName("M22222"));
        var student = _isuService.AddStudent(group1, "Ivan", "Ivanov");
        _isuService.ChangeStudentGroup(student, group2);
        Assert.Equal(student.Group, group2);
    }
}