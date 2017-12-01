using System;

using JetBrains.Annotations;

namespace Itsg.Ostc
{
    /// <summary>
    /// Absender-ID
    /// </summary>
    public class SenderId : IEquatable<SenderId>, IComparable<SenderId>, IComparable
    {
        private SenderId(SenderIdType type, [NotNull] string id)
        {
            Type = type;
            Id = id;
        }

        /// <summary>
        /// Art der Absender-ID
        /// </summary>
        public SenderIdType Type { get; }

        /// <summary>
        /// Nummer der Absender-ID
        /// </summary>
        [NotNull]
        public string Id { get; }

        /// <summary>
        /// Holt die Empfänger-ID für die Absender-ID
        /// </summary>
        /// <returns>Empfänger-ID für BNR oder IK</returns>
        [NotNull]
        public ReceiverId CommunicationServerReceiver
        {
            get
            {
                switch (Type)
                {
                    case SenderIdType.BNR:
                    case SenderIdType.ZNR:
                        return ReceiverId.OstcBnr;
                }
                return ReceiverId.OstcIk;
            }
        }

        /// <summary>
        /// Erstellt eine Absender-ID anhand einer BNR oder IK
        /// </summary>
        /// <param name="bnrOrIk">Eine ID die mit BN oder IK beginnt</param>
        /// <returns>Absender-ID</returns>
        /// <remarks>Sollte die <paramref name="bnrOrIk"/> nicht mit BN oder IK beginnen, dann wird angenommen, dass die ID eine Betriebsnummer ist.</remarks>
        public static SenderId FromBnrOrIk(string bnrOrIk)
        {
            return FromBnrOrIk(bnrOrIk, SenderIdType.BNR);
        }

        /// <summary>
        /// Erstellt eine Absender-ID anhand einer BNR oder IK
        /// </summary>
        /// <param name="bnrOrIk">Eine ID die mit BN oder IK beginnt</param>
        /// <param name="bnrType">Wenn die ID nicht mit BN oder IK beginnt, dann wird der hier angegebene ID-Typ verwendet</param>
        /// <returns>Absender-ID</returns>
        /// <remarks>Sollte die <paramref name="bnrOrIk"/> nicht mit BN oder IK beginnen, dann wird als ID-Typ der im <paramref name="bnrType"/> hinterlegte verwendet.</remarks>
        public static SenderId FromBnrOrIk(string bnrOrIk, SenderIdType bnrType)
        {
            var hasPrefixBN = bnrOrIk.StartsWith("BN", StringComparison.OrdinalIgnoreCase);
            var hasPrefix = hasPrefixBN || bnrOrIk.StartsWith("IK", StringComparison.OrdinalIgnoreCase);
            var type = !hasPrefix ? bnrType : (hasPrefixBN ? SenderIdType.BNR : SenderIdType.IK);
            var id = bnrOrIk.Substring(hasPrefix ? 2 : 0);
            return new SenderId(type, id);
        }

        /// <summary>
        /// Erstellt eine Absender-ID anhand einer Betriebsnummer.
        /// </summary>
        /// <param name="bnr">Die Betriebsnummer</param>
        /// <returns>Die Absender-ID</returns>
        public static SenderId FromBnr(string bnr)
        {
            var hasPrefix = bnr.StartsWith("BN", StringComparison.OrdinalIgnoreCase);
            return new SenderId(SenderIdType.BNR, bnr.Substring(hasPrefix ? 2 : 0));
        }

        /// <summary>
        /// Erstellt eine Absender-ID anhand einer IK.
        /// </summary>
        /// <param name="ik">Die IK</param>
        /// <returns>Die Absender-ID</returns>
        public static SenderId FromIk(string ik)
        {
            var hasPrefix = ik.StartsWith("IK", StringComparison.OrdinalIgnoreCase);
            return new SenderId(SenderIdType.IK, ik.Substring(hasPrefix ? 2 : 0));
        }

        /// <summary>
        /// Erstellt eine Absender-ID anhand einer ZNR.
        /// </summary>
        /// <param name="znr">Die ZNR</param>
        /// <returns>Die Absender-ID</returns>
        public static SenderId FromZnr(string znr)
        {
            var hasPrefix = znr.StartsWith("BN", StringComparison.OrdinalIgnoreCase);
            return new SenderId(SenderIdType.ZNR, znr.Substring(hasPrefix ? 2 : 0));
        }

        /// <summary>
        /// Zeigt die Absender-ID als String an.
        /// </summary>
        /// <returns>Die Absender-ID in der Form {IK|BN}[0-9]+</returns>
        public override string ToString()
        {
            if (Type == SenderIdType.ZNR)
                return $"{"BN"}{Id}";
            return $"{Type.ToString().Substring(0, 2)}{Id}";
        }

        /// <summary>
        /// Prüft, ob diese Absender-ID mit der übergebenen gleich ist.
        /// </summary>
        /// <param name="obj">Die andere Absender-ID gegen die verglichen wird</param>
        /// <returns>true, wenn beide Absender-IDs gleich sind</returns>
        public override bool Equals(object obj)
        {
            return Equals((SenderId)obj);
        }

        /// <summary>
        /// Der Hash-Code dieser Absender-ID
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode()
                ^ Id.GetHashCode();
        }

        /// <summary>
        /// Prüft, ob diese Absender-ID mit der übergebenen gleich ist.
        /// </summary>
        /// <param name="other">Die andere Absender-ID gegen die verglichen wird</param>
        /// <returns>true, wenn beide Absender-IDs gleich sind</returns>
        public bool Equals(SenderId other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return Type == other.Type && Id == other.Id;
        }

        /// <summary>
        /// Vergleicht, ob diese Absender-ID sich von der anderen unterscheidet
        /// </summary>
        /// <param name="obj">Die andere Absender-ID</param>
        /// <returns>0 = Absender-IDs sind identisch, &lt;0 = Diese Absender-ID ist kleiner, &gt;0 = Diese Absender-ID ist größer</returns>
        public int CompareTo(object obj)
        {
            return CompareTo((SenderId)obj);
        }

        /// <summary>
        /// Vergleicht, ob diese Absender-ID sich von der anderen unterscheidet
        /// </summary>
        /// <param name="other">Die andere Absender-ID</param>
        /// <returns>0 = Absender-IDs sind identisch, &lt;0 = Diese Absender-ID ist kleiner, &gt;0 = Diese Absender-ID ist größer</returns>
        public int CompareTo(SenderId other)
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
        /// <param name="a">Absender-ID 1</param>
        /// <param name="b">Absender-ID 2</param>
        /// <returns>true, wenn die Absender-IDs identisch sind</returns>
        public static bool operator ==(SenderId a, SenderId b)
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
        /// <param name="a">Absender-ID 1</param>
        /// <param name="b">Absender-ID 2</param>
        /// <returns>true, wenn die Absender-IDs unterschiedlich sind</returns>
        public static bool operator !=(SenderId a, SenderId b)
        {
            return !(a == b);
        }
    }
}
