namespace SH.MapGenerator.Generators.Blends
{
    public class LerpBlendGenerator : BaseBlendGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("LerpBlend");
    }
}