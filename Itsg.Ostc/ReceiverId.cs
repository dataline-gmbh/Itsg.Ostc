using System;

namespace Itsg.Ostc
{
    /// <summary>
    /// Die Empfänger-ID
    /// </summary>
    public class ReceiverId : IEquatable<ReceiverId>, IComparable<ReceiverId>, IComparable
    {
        /// <summary>
        /// Empfänger-ID für die OSTC, Betriebsnummer
        /// </summary>
        public static ReceiverId OstcBnr { get; private set; }

        /// <summary>
        /// Empfänger-ID für die OSTC, IK-Nummer
        /// </summary>
        public static ReceiverId OstcIk { get; private set; }

        static ReceiverId()
        {
            OstcBnr = FromBnr("17046976");
            OstcIk = FromIk("660640162");
        }

        private ReceiverId(ReceiverIdType type, string id)
        {
            Id = id;
            Type = type;
        }

        /// <summary>
        /// Holt den ID-Typ
        /// </summary>
        public ReceiverIdType Type { get; }

        /// <summary>
        /// Holt die Nummer für den ID-Typen
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Erstellt eine Empfänger-ID anhand einer BNR oder IK
        /// </summary>
        /// <param name="bnrOrIk">Eine ID die mit BN oder IK beginnt</param>
        /// <returns>Empfänger-ID</returns>
        /// <remarks>Sollte die <paramref name="bnrOrIk"/> nicht mit BN oder IK beginnen, dann wird angenommen, dass die ID eine Betriebsnummer ist.</remarks>
        public static ReceiverId FromBnrOrIk(string bnrOrIk)
        {
            return FromBnrOrIk(bnrOrIk, ReceiverIdType.BNR);
        }

        /// <summary>
        /// Erstellt eine Empfänger-ID anhand einer BNR oder IK
        /// </summary>
        /// <param name="bnrOrIk">Eine ID die mit BN oder IK beginnt</param>
        /// <param name="bnrType">Wenn die ID nicht mit BN oder IK beginnt, dann wird der hier angegebene ID-Typ verwendet</param>
        /// <returns>Empfänger-ID</returns>
        /// <remarks>Sollte die <paramref name="bnrOrIk"/> nicht mit BN oder IK beginnen, dann wird als ID-Typ der im <paramref name="bnrType"/> hinterlegte verwendet.</remarks>
        public static ReceiverId FromBnrOrIk(string bnrOrIk, ReceiverIdType bnrType)
        {
            var hasPrefixBN = bnrOrIk.StartsWith("BN", StringComparison.OrdinalIgnoreCase);
            var hasPrefix = hasPrefixBN || bnrOrIk.StartsWith("IK", StringComparison.OrdinalIgnoreCase);
            var type = !hasPrefix ? bnrType : (hasPrefixBN ? ReceiverIdType.BNR : ReceiverIdType.IK);
            var id = bnrOrIk.Substring(hasPrefix ? 2 : 0);
            return new ReceiverId(type, id);
        }

        /// <summary>
        /// Erstellt eine Empfänger-ID anhand einer Betriebsnummer.
        /// </summary>
        /// <param name="bnr">Die Betriebsnummer</param>
        /// <returns>Die Empfänger-ID</returns>
        public static ReceiverId FromBnr(string bnr)
        {
            var hasPrefix = bnr.StartsWith("BN", StringComparison.OrdinalIgnoreCase);
            return new ReceiverId(ReceiverIdType.BNR, bnr.Substring(hasPrefix ? 2 : 0));
        }

        /// <summary>
        /// Erstellt eine Empfänger-ID anhand einer IK.
        /// </summary>
        /// <param name="ik">Die IK</param>
        /// <returns>Die Empfänger-ID</returns>
        public static ReceiverId FromIk(string ik)
        {
            var hasPrefix = ik.StartsWith("IK", StringComparison.OrdinalIgnoreCase);
            return new ReceiverId(ReceiverIdType.IK, ik.Substring(hasPrefix ? 2 : 0));
        }

        /// <summary>
        /// Zeigt die Empfänger-ID als String an.
        /// </summary>
        /// <returns>Die Empfänger-ID in der Form {IK|BN}[0-9]+</returns>
        public override string ToString()
        {
            return $"{Type.ToString().Substring(0, 2)}{Id}";
        }

        /// <summary>
        /// Prüft, ob diese Empfänger-ID mit der übergebenen gleich ist.
        /// </summary>
        /// <param name="obj">Die andere Empfänger-ID gegen die verglichen wird</param>
        /// <returns>true, wenn beide Empfänger-IDs gleich sind</returns>
        public override bool Equals(object obj)
        {
            return Equals((ReceiverId)obj);
        }

        /// <summary>
        /// Der Hash-Code dieser Empfänger-ID
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode()
                ^ Id.GetHashCode();
        }

        /// <summary>
        /// Prüft, ob diese Empfänger-ID mit der übergebenen gleich ist.
        /// </summary>
        /// <param name="other">Die andere Empfänger-ID gegen die verglichen wird</param>
        /// <returns>true, wenn beide Empfänger-IDs gleich sind</returns>
        public bool Equals(ReceiverId other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return Type == other.Type && Id == other.Id;
        }

        /// <summary>
        /// Vergleicht, ob diese Empfänger-ID sich von der anderen unterscheidet
        /// </summary>
        /// <param name="obj">Die andere Empfänger-ID</param>
        /// <returns>0 = Empfänger-IDs sind identisch, &lt;0 = Diese Empfänger-ID ist kleiner, &gt;0 = Diese Empfänger-ID ist größer</returns>
        public int CompareTo(object obj)
        {
            return CompareTo((ReceiverId)obj);
        }

        /// <summary>
        /// Vergleicht, ob diese Empfänger-ID sich von der anderen unterscheidet
        /// </summary>
        /// <param name="other">Die andere Empfänger-ID</param>
        /// <returns>0 = Empfänger-IDs sind identisch, &lt;0 = Diese Empfänger-ID ist kleiner, &gt;0 = Diese Empfänger-ID ist größer</returns>
        public int CompareTo(ReceiverId other)
        {
            var result = Type.CompareTo(other.Type);
            if (result != 0)
                return result;
            result = StringComparer.Ordinal.Compare(Id, other.Id);
            return result;
        }

        /// <summary>
        /// Operator für Vergleiche auf Gleichheit
        /// </summary>
        /// <param name="a">Empfänger-ID 1</param>
        /// <param name="b">Empfänger-ID 2</param>
        /// <returns>true, wenn die Empfänger-IDs identisch sind</returns>
        public static bool operator ==(ReceiverId a, ReceiverId b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        /// <summary>
        /// Operator für Vergleiche auf Ungleichheit
        /// </summary>
        /// <param name="a">Empfänger-ID 1</param>
        /// <param name="b">Empfänger-ID 2</param>
        /// <returns>true, wenn die Empfänger-IDs unterschiedlich sind</returns>
        public static bool operator !=(ReceiverId a, ReceiverId b)
        {
            return !(a == b);
        }
    }
}
