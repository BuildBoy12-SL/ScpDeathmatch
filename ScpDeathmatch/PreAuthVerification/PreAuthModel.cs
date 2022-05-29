// -----------------------------------------------------------------------
// <copyright file="PreAuthModel.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace ScpDeathmatch.PreAuthVerification
{
    using System;
    using LiteNetLib.Utils;

    /// <summary>
    /// Represents a pre-auth request.
    /// </summary>
    public class PreAuthModel
    {
        /// <summary>
        /// Gets a value indicating whether the authentication provided a challenge.
        /// </summary>
        public bool IsChallenge { get; private set; }

        /// <summary>
        /// Gets the leading byte of the request.
        /// </summary>
        public byte PlaceholderByte { get; private set; }

        /// <summary>
        /// Gets the major version.
        /// </summary>
        public byte Major { get; private set; }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        public byte Minor { get; private set; }

        /// <summary>
        /// Gets the revision version.
        /// </summary>
        public byte Revision { get; private set; }

        /// <summary>
        /// Gets the backward revision version.
        /// </summary>
        public byte BackwardRevision { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the authentication provided a flag.
        /// </summary>
        public bool Flag { get; private set; }

        /// <summary>
        /// Gets the id of the challenge.
        /// </summary>
        public int ChallengeID { get; private set; }

        /// <summary>
        /// Gets the challenge sent during authentication.
        /// </summary>
        public byte[] Challenge { get; private set; }

        /// <summary>
        /// Gets the user id of the authenticating user.
        /// </summary>
        public string UserID { get; private set; } = "Unknown UserID";

        /// <summary>
        /// Gets the expiration date of the authentication.
        /// </summary>
        public long Expiration { get; private set; }

        /// <summary>
        /// Gets the authentication flags.
        /// </summary>
        public byte Flags { get; private set; }

        /// <summary>
        /// Gets the region the authentication originated from.
        /// </summary>
        public string Region { get; private set; } = "Unknown Region";

        /// <summary>
        /// Gets the signature of the authentication.
        /// </summary>
        public byte[] Signature { get; private set; } = Array.Empty<byte>();

        /// <summary>
        /// Reads an authentication request and returns a <see cref="PreAuthModel"/>.
        /// </summary>
        /// <param name="reader">The authentication to read.</param>
        /// <returns>The <see cref="PreAuthModel"/> or null if it is invalid.</returns>
        public static PreAuthModel ReadPreAuth(NetDataReader reader)
        {
            PreAuthModel model = new PreAuthModel();

            if (reader.TryGetByte(out byte b))
                model.PlaceholderByte = b;

            byte cBackwardRevision = 0;
            byte cMajor;
            byte cMinor;
            byte cRevision;
            bool cflag;
            if (!reader.TryGetByte(out cMajor) || !reader.TryGetByte(out cMinor) || !reader.TryGetByte(out cRevision) ||
                !reader.TryGetBool(out cflag) || (cflag && !reader.TryGetByte(out cBackwardRevision)))
            {
                return null;
            }

            model.Major = cMajor;
            model.Minor = cMinor;
            model.Revision = cRevision;
            model.BackwardRevision = cBackwardRevision;
            model.Flag = cflag;

            if (reader.TryGetInt(out int challengeID))
            {
                model.IsChallenge = true;
                model.ChallengeID = challengeID;
            }

            if (reader.TryGetBytesWithLength(out byte[] challenge))
                model.Challenge = challenge;
            if (reader.TryGetString(out string userid))
                model.UserID = userid;
            if (reader.TryGetLong(out long expiration))
                model.Expiration = expiration;
            if (reader.TryGetByte(out byte flags))
                model.Flags = flags;
            if (reader.TryGetString(out string region))
                model.Region = region;
            if (reader.TryGetBytesWithLength(out byte[] signature))
                model.Signature = signature;

            return model;
        }
    }
}