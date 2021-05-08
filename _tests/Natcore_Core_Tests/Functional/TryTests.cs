using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Natcore.Core.Functional;
using System;
using System.Collections.Generic;
using System.Text;

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
				var success = Mock.Of<Func<string, int>>();
				var failure = Mock.Of<Func<Exception, int>>();

				Try.Action(() => "result").Match(success, failure);

				Mock.Get(success).Verify(_ => _("result"), Times.Once);
				Mock.Get(failure).Verify(_ => _(It.IsAny<Exception>()), Times.Never);
			}

			[TestMethod]
			public void Action_with_exception_capture_result()
			{
				var success = Mock.Of<Func<string, int>>();
				var failure = Mock.Of<Func<Exception, int>>();

				Exception ex = new Exception("exception");
				Try.Action<string>(() => throw ex).Match(success, failure);

				Mock.Get(success).Verify(_ => _(It.IsAny<string>()), Times.Never);
				Mock.Get(failure).Verify(_ => _(ex), Times.Once);
			}
		}

		[TestClass]
		public class AsSuccessStaticMethod
		{
			[TestMethod]
			public void Try_has_result()
			{
				var success = Mock.Of<Func<string, int>>();
				var failure = Mock.Of<Func<Exception, int>>();

				Try.AsSuccess("result").Match(success, failure);

				Mock.Get(success).Verify(_ => _("result"), Times.Once);
				Mock.Get(failure).Verify(_ => _(It.IsAny<Exception>()), Times.Never);
			}
		}

		[TestClass]
		public class AsFailureStaticMethod
		{
			[TestMethod]
			public void Try_has_error()
			{
				var success = Mock.Of<Func<string, int>>();
				var failure = Mock.Of<Func<Exception, int>>();

				Exception ex = new Exception("exception");
				Try.AsFailure<string>(ex).Match(success, failure);

				Mock.Get(success).Verify(_ => _(It.IsAny<string>()), Times.Never);
				Mock.Get(failure).Verify(_ => _(ex), Times.Once);
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
					var success = Mock.Of<Func<string, string>>();
					var failure = Mock.Of<Func<Exception, string>>();

					Try.AsSuccess("result")
						.Select((_) => "mapped result")
						.Match(success, failure);

					Mock.Get(success).Verify(_ => _("mapped result"), Times.Once);
					Mock.Get(failure).Verify(_ => _(It.IsAny<Exception>()), Times.Never);
				}

				[TestMethod]
				public void Select_on_failed_try_is_same_failure()
				{
					var success = Mock.Of<Func<string, string>>();
					var failure = Mock.Of<Func<Exception, string>>();

					Exception ex = new Exception("failure");
					var output = Try.AsFailure<string>(ex)
						.Select((_) => "mapped result");
					output.Match(success, failure);

					Mock.Get(success).Verify(_ => _(It.IsAny<string>()), Times.Never);
					Mock.Get(failure).Verify(_ => _(ex), Times.Once);
				}

				[TestMethod]
				public void Can_use_query_syntax_on_successful_try()
				{
					var success = Mock.Of<Func<string, string>>();
					var failure = Mock.Of<Func<Exception, string>>();

					var query = from x in Try.AsSuccess("result")
								select "mapped result";

					query.Match(success, failure);

					Mock.Get(success).Verify(_ => _("mapped result"), Times.Once);
					Mock.Get(failure).Verify(_ => _(It.IsAny<Exception>()), Times.Never);
				}
			}

			[TestClass]
			public class SelectManyMethod
			{
				[TestMethod]
				public void Result_is_successful_if_both_try_are_successful()
				{
					var success = Mock.Of<Func<object, string>>();
					var failure = Mock.Of<Func<Exception, string>>();

					var query = from a in Try.AsSuccess("result")
								from b in Try.AsSuccess(2)
								select new { a, b };

					query.Match(success, failure);

					Mock.Get(success).Verify(_ => _(new { a = "result", b = 2 }), Times.Once);
					Mock.Get(failure).Verify(_ => _(It.IsAny<Exception>()), Times.Never);
				}

				[TestMethod]
				public void Result_is_as_failure_if_first_try_is_failure()
				{
					var success = Mock.Of<Func<object, string>>();
					var failure = Mock.Of<Func<Exception, string>>();

					var e1 = new Exception("e1");
					var e2 = new Exception("e2");

					var query = from a in Try.AsFailure<string>(e1)
								from b in Try.AsFailure<int>(e2)
								select new { a, b };

					query.Match(success, failure);

					Mock.Get(success).Verify(_ => _(It.IsAny<object>()), Times.Never);
					Mock.Get(failure).Verify(_ => _(e1), Times.Once);
				}

				[TestMethod]
				public void Result_is_as_failure_if_second_try_is_failure()
				{
					var success = Mock.Of<Func<object, string>>();
					var failure = Mock.Of<Func<Exception, string>>();

					var e2 = new Exception("e2");

					var query = from a in Try.AsSuccess<string>("result")
								from b in Try.AsFailure<int>(e2)
								select new { a, b };

					query.Match(success, failure);

					Mock.Get(success).Verify(_ => _(It.IsAny<object>()), Times.Never);
					Mock.Get(failure).Verify(_ => _(e2), Times.Once);
				}
			}
		}
	}
}
