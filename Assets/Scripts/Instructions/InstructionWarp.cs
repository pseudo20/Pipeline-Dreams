namespace PipelineDreams
{
    public class InstructionWarp : Instruction {
        public InstructionWarp(EntityDataContainer eM, Entity player, CommandsContainer pC, InstructionData data, string variant) : base(eM, player, pC, data, variant) {
        }

        public override IClockTask Operation(float startClock)
        {

            return new InstructionWarpTask();
        }
        
    }
    public abstract partial class Instruction
    {
        /// <summary>
        /// Field instruction task used above.
        /// </summary>
        protected class InstructionWarpTask : InstructionTask
        {
            protected override void OnRunStart()
            {
            }
        }
    }

}
