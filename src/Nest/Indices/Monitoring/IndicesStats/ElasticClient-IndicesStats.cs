﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Nest
{
	public partial interface IElasticClient
	{
		/// <summary>
		/// Indices level stats provide statistics on different operations happening on an index. The API provides statistics on 
		/// the index level scope (though most stats can also be retrieved using node level scope).
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/indices-stats.html
		/// </summary>
		/// <param name="selector">Optionaly further describe the indices stats operation</param>
		IGlobalStatsResponse IndicesStats(Func<IndicesStatsDescriptor, IIndicesStatsRequest> selector = null);

		/// <inheritdoc/>
		IGlobalStatsResponse IndicesStats(IIndicesStatsRequest indicesStatsRequest);

		/// <inheritdoc/>
		Task<IGlobalStatsResponse> IndicesStatsAsync(Func<IndicesStatsDescriptor, IIndicesStatsRequest> selector = null);

		/// <inheritdoc/>
		Task<IGlobalStatsResponse> IndicesStatsAsync(IIndicesStatsRequest indicesStatsRequest);

	}

	public partial class ElasticClient
	{
		/// <inheritdoc/>
		public IGlobalStatsResponse IndicesStats(Func<IndicesStatsDescriptor, IIndicesStatsRequest> selector = null) => 
			this.Dispatcher.Dispatch<IIndicesStatsRequest, IndicesStatsRequestParameters, GlobalStatsResponse>(
				selector.InvokeOrDefault(new IndicesStatsDescriptor()),
				(p, d) => this.LowLevelDispatch.IndicesStatsDispatch<GlobalStatsResponse>(p)
			);

		/// <inheritdoc/>
		public IGlobalStatsResponse IndicesStats(IIndicesStatsRequest statsRequest) => 
			this.Dispatcher.Dispatch<IIndicesStatsRequest, IndicesStatsRequestParameters, GlobalStatsResponse>(
				statsRequest,
				(p, d) => this.LowLevelDispatch.IndicesStatsDispatch<GlobalStatsResponse>(p)
			);

		/// <inheritdoc/>
		public Task<IGlobalStatsResponse> IndicesStatsAsync(Func<IndicesStatsDescriptor, IIndicesStatsRequest> selector = null) => 
			this.Dispatcher.DispatchAsync<IIndicesStatsRequest, IndicesStatsRequestParameters, GlobalStatsResponse, IGlobalStatsResponse>(
				selector.InvokeOrDefault(new IndicesStatsDescriptor()),
				(p, d) => this.LowLevelDispatch.IndicesStatsDispatchAsync<GlobalStatsResponse>(p)
			);

		/// <inheritdoc/>
		public Task<IGlobalStatsResponse> IndicesStatsAsync(IIndicesStatsRequest statsRequest) => 
			this.Dispatcher.DispatchAsync<IIndicesStatsRequest, IndicesStatsRequestParameters, GlobalStatsResponse, IGlobalStatsResponse>(
				statsRequest,
				(p, d) => this.LowLevelDispatch.IndicesStatsDispatchAsync<GlobalStatsResponse>(p)
			);
	}
}