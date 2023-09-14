﻿namespace ATAS.Indicators.Technical
{
    using System;
    using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
	using System.Windows.Media;

	using OFT.Attributes;
    using OFT.Localization;

    [DisplayName("Starc Bands")]
	[HelpLink("https://support.atas.net/knowledge-bases/2/articles/45497-starc-bands")]
	public class StarcBands : Indicator
	{
		#region Fields

		private readonly ATR _atr = new() { Period = 10 };
        private readonly SMA _sma = new() { Period = 10 };

        private readonly ValueDataSeries _botSeries = new("BotSeries", Strings.BottomBand)
		{
			Color = Colors.DodgerBlue,
			IgnoredByAlerts = true
		};

		private readonly ValueDataSeries _smaSeries = new("SmaSeries", Strings.SMA);
		private readonly ValueDataSeries _topSeries = new("TopSeries", Strings.TopBand)
		{
			Color = Colors.DodgerBlue,
			IgnoredByAlerts = true
        };

		private decimal _botBand = 1;
        private decimal _topBand = 1;

        #endregion

        #region Properties

        [Parameter]
        [Display(ResourceType = typeof(Strings), Name = nameof(Strings.Period), GroupName = nameof(Strings.Settings), Order = 100)]
		[Range(1, 1000000)]
		public int Period
		{
			get => _sma.Period;
			set
			{
				_sma.Period = value;
				RecalculateValues();
			}
		}

        [Parameter]
        [Display(ResourceType = typeof(Strings), Name = nameof(Strings.ATR), GroupName = nameof(Strings.Settings), Order = 110)]
		[Range(1, 1000000)]
		public int SmaPeriod
		{
			get => _atr.Period;
			set
			{
				_atr.Period = value;
				RecalculateValues();
			}
		}

        [Parameter]
        [Display(ResourceType = typeof(Strings), Name = nameof(Strings.TopBand), GroupName = nameof(Strings.BBandsWidth), Order = 200)]
		[Range(0.000001, 1000000)]
		public decimal TopBand
		{
			get => _topBand;
			set
			{
				_topBand = value;
				RecalculateValues();
			}
		}

        [Parameter]
        [Display(ResourceType = typeof(Strings), Name = nameof(Strings.BottomBand), GroupName = nameof(Strings.BBandsWidth), Order = 210)]
		[Range(0.000001, 1000000)]
		public decimal BotBand
		{
			get => _botBand;
			set
			{
				_botBand = value;
				RecalculateValues();
			}
		}

		#endregion

		#region ctor

		public StarcBands()
		{
			Add(_atr);

			DataSeries[0] = _topSeries;
			DataSeries.Add(_botSeries);
			DataSeries.Add(_smaSeries);
		}

		#endregion

		#region Protected methods

		protected override void OnCalculate(int bar, decimal value)
		{
			_sma.Calculate(bar, value);

			var bandValue = _topBand * _atr[bar];

			_topSeries[bar] = _sma[bar] + bandValue;
			_botSeries[bar] = _sma[bar] - bandValue;
			_smaSeries[bar] = _sma[bar];
		}

		#endregion
	}
}