namespace DatEx.ApsTender.DataModel.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    /// <summary> Решение по лоту </summary>
    public enum ELotSolution

    {
        /// <summary> Решение не выбрано </summary>
        WithoutSolution = 0,

        /// <summary> Завершен с победителем </summary>
        FinishedWithWinner = 1,

        /// <summary> Перенесен на слудующий тур </summary>
        ToTheNextRound = 2,

        /// <summary> </summary>
        FinishedWithoutWinner = 3
    }
}
