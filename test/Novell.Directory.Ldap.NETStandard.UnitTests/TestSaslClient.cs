﻿using Novell.Directory.Ldap.Sasl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Novell.Directory.Ldap.NETStandard.UnitTests
{
    public class TestSaslClientFactory : ISaslClientFactory
    {
        public TestSaslClientFactory(string mechanism)
        {
            SupportedMechanisms = new[] { mechanism };
        }

        public IReadOnlyList<string> SupportedMechanisms { get; }

        public ISaslClient CreateClient(SaslRequest saslRequest)
        {
            return new TestSaslClient(saslRequest?.SaslMechanism);
        }
    }

    public sealed class TestSaslClient : ISaslClient
    {
        public DebugId DebugId { get; } = DebugId.ForType<TestSaslClient>();

        public TestSaslClient(string mechanism)
        {
            MechanismName = mechanism;
        }

        public string MechanismName { get; }

        public bool HasInitialResponse => false;

        public bool IsComplete => true;

        public void Dispose()
        {
        }

        public Task<byte[]> EvaluateChallengeAsync(byte[] challenge)
        {
            return Task.FromResult(challenge);
        }
    }
}
