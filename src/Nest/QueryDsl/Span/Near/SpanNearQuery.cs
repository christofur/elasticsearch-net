﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nest.Resolvers.Converters;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeConverter<SpanNearQueryDescriptor<object>>))]
	public interface ISpanNearQuery : ISpanSubQuery
	{
		[JsonProperty(PropertyName = "clauses")]
		IEnumerable<ISpanQuery> Clauses { get; set; }

		[JsonProperty(PropertyName = "slop")]
		int? Slop { get; set; }

		[JsonProperty(PropertyName = "in_order")]
		bool? InOrder { get; set; }

		[JsonProperty(PropertyName = "collect_payloads")]
		bool? CollectPayloads { get; set; }
	}

	public class SpanNearQuery : QueryBase, ISpanNearQuery
	{
		bool IQuery.Conditionless { get { return false; } }
		public IEnumerable<ISpanQuery> Clauses { get; set; }
		public int? Slop { get; set; }
		public bool? InOrder { get; set; }
		public bool? CollectPayloads { get; set; }

		protected override void WrapInContainer(IQueryContainer c) => c.SpanNear = this;
		internal static bool IsConditionless(ISpanNearQuery q) => !q.Clauses.HasAny() || q.Clauses.Cast<IQuery>().All(qq => qq.Conditionless);
	}

	public class SpanNearQueryDescriptor<T> 
		: QueryDescriptorBase<SpanNearQueryDescriptor<T>, ISpanNearQuery>
		, ISpanNearQuery where T : class
	{
		private ISpanNearQuery Self => this;
		bool IQuery.Conditionless => SpanNearQuery.IsConditionless(this);
		IEnumerable<ISpanQuery> ISpanNearQuery.Clauses { get; set; }
		int? ISpanNearQuery.Slop { get; set; }
		bool? ISpanNearQuery.InOrder { get; set; }
		bool? ISpanNearQuery.CollectPayloads { get; set; }

		public SpanNearQueryDescriptor<T> Clauses(params Func<SpanQueryDescriptor<T>, SpanQueryDescriptor<T>>[] selectors)
		{
			selectors.ThrowIfNull("selector");
			var descriptors = new List<SpanQueryDescriptor<T>>();
			foreach (var selector in selectors)
			{
				var x = new SpanQueryDescriptor<T>();
				var q = selector(x);
				if ((q as IQuery).Conditionless)
					continue;

				descriptors.Add(q);

			}
			Self.Clauses = descriptors.HasAny() ? descriptors : null;
			return this;
		}

		public SpanNearQueryDescriptor<T> Slop(int slop)
		{
			Self.Slop = slop;
			return this;
		}

		public SpanNearQueryDescriptor<T> InOrder(bool inOrder)
		{
			Self.InOrder = inOrder;
			return this;
		}

		public SpanNearQueryDescriptor<T> CollectPayloads(bool collectPayloads)
		{
			Self.CollectPayloads = collectPayloads;
			return this;
		}
	}
}