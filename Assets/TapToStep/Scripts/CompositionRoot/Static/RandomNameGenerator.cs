namespace CompositionRoot.Static
{
    public static class RandomNameGenerator
    {
    private static readonly string[] r_names =
    {
        "Dash", "Blaze", "Neon", "Turbo", "Storm",
        "Pixel", "Cyber", "Rocket", "Phantom", "Flash",
        "Eclipse", "Velocity", "Thunder", "Warp", "Nova",
        "Phantom", "Aero", "Sonic", "Lunar", "Solar",
        "Hyper", "Starlight", "Ghost", "Inferno", "Zenith",
        "Quantum", "Solar", "Astral", "Zero", "Meteor",
        "Neon", "Glitch", "Echo", "Circuit", "Star",
        "Celestial", "Eclipse", "Orbit", "Speed", "Supernova",
        "Aurora", "Dark", "Warp", "Gravity", "Titan",
        "Comet", "Turbo", "Rapid", "Skybound", "Horizon",
        "Infinity", "Supersonic", "Warp", "Overdrive", "Astro",
        "Neon", "Fusion", "Pulsar", "Echo", "Ion",
        "Thunder", "Galactic", "Skybolt", "Quantum", "Blaze",
        "Rocket", "Sonic", "Void", "Cosmic", "Lightning",
        "Friction", "Storm", "Nova", "Horizon", "Hyper",
        "Infinity", "Nebula", "Zero", "Solar", "Lunar",
        "Circuit", "Vortex", "Flash", "Sky", "Quantum",
        "Neon", "Ether", "Titan", "Horizon", "Ghost",
        "Celestial", "Nitro", "Speed", "Dynamo", "Warp",
        "Infinity", "Starlight", "Turbo", "Galactic", "Cyber",
        "Stormy", "Orbit", "Vortex", "RocketX", "EchoX",
        "Drift", "Glider", "Frost", "Bolt", "Spark",
        "Shadow", "AstroX", "Zephyr", "Skylark", "Axion",
        "Blitz", "Flare", "NovaX", "MeteorX", "SonicX",
        "StormX", "LunarX", "CyberX", "HyperX", "Zypher",
        "Havoc", "Raptor", "VelocityX", "NebulaX", "PulsarX",
        "CometX", "GravityX", "PhantomX", "TitanX", "HorizonX",
        "DynamoX", "CelestialX", "WarpX", "InfinityX", "StarlightX",
        "VortexX", "SkyboltX", "NeonX", "CircuitX", "AuroraX",
        "LightningX", "GhostX", "ZenithX", "InfernoX", "FusionX",
        "EclipseX", "SupernovaX", "OverdriveX", "BlazeX", "TurboX",
        "QuantumX", "SonicDrift", "NovaDrift", "WarpDrift", "VelocityDrift",
        "AstroDrift", "ThunderDrift", "StormDrift", "ZeroDrift", "LunarDrift", "HyperDrift",
        "SkyDrift", "SolarDrift", "FrictionX", "EchoDrift", "HorizonDrift",
        "PulsarDrift", "BlitzDrift", "FlashDrift", "ShadowDrift", "AxionDrift"
    };
    
        public static string GetRandomName()
        {
            return r_names[UnityEngine.Random.Range(0, r_names.Length)];
        }
    }
}