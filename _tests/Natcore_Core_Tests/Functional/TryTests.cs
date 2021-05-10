using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Natcore.Core.Functional;
using System;

namespace Natcore_Core_Tests.Functional
{
	[TestClass]
	public class TryTests
	{
		[TestClass]
		public class ActionStaticMethod
		{
			[TestMethod]
			public void Action_with_no_errors_capture_result()
			{
				Try.Action(() => "result")
					.Should()
					.Be(new TrySuccess<string>("result"));
			}

			[TestMethod]
			public void Action_with_exception_capture_result()
			{
				Exception ex = new ("exception");
				Try.Action<string>(() => throw ex)
					.Should()
					.Be(new TryFailure<string>(ex));
			}
		}

		[TestClass]
		public class AsSuccessStaticMethod
		{
			[TestMethod]
			public void Try_has_result()
			{
				Try.AsSuccess("result")
					.Should()
					.Be(new TrySuccess<string>("result"));
			}
		}

		[TestClass]
		public class AsFailureStaticMethod
		{
			[TestMethod]
			public void Try_has_error()
			{
				Exception ex = new ("exception");
				Try.AsFailure<string>(ex)
					.Should()
					.Be(new TryFailure<string>(ex));
			}
		}

		[TestClass]
		public class LinqExtensionTests
		{
			[TestClass]
			public class SelectMethod
			{
				[TestMethod]
				public void Select_on_successful_try_maps_value()
				{
					Try.AsSuccess("result")
						.Select((_) => "mapped result")
						.Should()
						.Be(new TrySuccess<string>("mapped result"));
				}

				[TestMethod]
				public void Select_on_failed_try_is_same_failure()
				{
					Exception ex = new ("failure");
					var output = Try.AsFailure<string>(ex)
						.Select((_) => "mapped result")
						.Should()
						.Be(new TryFailure<string>(ex));
				}

				[TestMethod]
				public void Can_use_query_syntax_on_successful_try()
				{
					var query = from x in Try.AsSuccess("result")
								select "mapped result";

					query.Should().Be(new TrySuccess<string>("mapped result"));
				}
			}

			[TestClass]
			public class SelectManyMethod
			{
				[TestMethod]
				public void Result_is_successful_if_both_try_are_successful()
				{
					var query = from a in Try.AsSuccess("result")
								from b in Try.AsSuccess(2)
								select (a, b);

					query.Should().Be(new TrySuccess<(string a, int b)>(("result", 2)));
				}

				[TestMethod]
				public void Result_is_as_failure_if_first_try_is_failure()
				{
					var e1 = new Exception("e1");
					var e2 = new Exception("e2");

					var query = from a in Try.AsFailure<string>(e1)
								from b in Try.AsFailure<int>(e2)
								select (a, b);

					query.Should().Be(new TryFailure<(string a, int b)>(e1));
				}

				[TestMethod]
				public void Result_is_as_failure_if_second_try_is_failure()
				{
					var e2 = new Exception("e2");

					var query = from a in Try.AsSuccess("result")
								from b in Try.AsFailure<int>(e2)
								select (a, b);

					query.Should().Be(new TryFailure<(string a, int b)>(e2));
				}
			}
		}
	}
}
