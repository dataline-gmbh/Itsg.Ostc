using System.Collections.Generic;
using System.Linq;

using ExtraStandard;
using ExtraStandard.Extra11;

using Itsg.Ostc;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Basis-Exception für den OSTC-Client
    /// </summary>
    public class Ostc2Exception : OstcException
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="flags">eXTra-Fehler</param>
        public Ostc2Exception(IReadOnlyCollection<ExtraFlag> flags)
            : base(string.Join("\n", flags.Select(x => $"{x.Text} ({x.Code})")))
        {
            Flags = flags;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="flags">eXTra-Fehler</param>
        public Ostc2Exception(IEnumerable<FlagType> flags)
            : this(flags.Select(x => x.AsExtraFlag()).ToList())
        {
        }

        /// <summary>
        /// eXTra-Fehler
        /// </summary>
        public IReadOnlyCollection<ExtraFlag> Flags { get; private set; }
    }
}
