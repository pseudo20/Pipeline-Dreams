namespace PipelineDreams
{
    public class InstructionRay : Instruction {
        public InstructionRay(EntityDataContainer eM, Entity player, CommandsContainer pC, InstructionData data, string variant) : base(eM, player, pC, data, variant) {
        }

        public override IClockTask Operation(float startClock)
        {

            return new InstructionRayTask();
        }
        
    }
    public abstract partial class Instruction
    {
        /// <summary>
        /// Field instruction task used above.
        /// </summary>
        protected class InstructionRayTask : InstructionTask
        {
            protected override void OnRunStart()
            {
            }
        }
    }

}
