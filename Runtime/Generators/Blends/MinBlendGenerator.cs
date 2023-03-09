namespace SH.MapGenerator.Generators.Blends
{
    public class MinBlendGenerator : BaseBlendGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("MinBlend");
    }
}