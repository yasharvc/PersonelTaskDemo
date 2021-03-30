using DomainLayer.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Unitests.Domain
{
	public class BaseEntityTests
	{
		class TestEntity : BaseEntity
		{

		}
		[Fact]
		public void When_Inistanciate_Should_Created_Has_Valid_DateTime()
		{
			var beforeCreationDateTime = DateTime.Now;
			var entity = new TestEntity();

			Assert.True(entity.Created.CompareTo(beforeCreationDateTime) > 0);
		}
		[Fact]
		public void When_Inistanciate_Should_Id_Not_Empty()
		{
			var entity = new TestEntity();

			Assert.False(string.IsNullOrEmpty(entity.Id));
		}
	}
}