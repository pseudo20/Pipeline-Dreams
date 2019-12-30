namespace PipelineDreams
{
    public class InstructionEject : Instruction {
        public InstructionEject(EntityDataContainer eM, Entity player, CommandsContainer pC, InstructionData data, string variant) : base(eM, player, pC, data, variant) {
        }

        public override IClockTask Operation(float startClock)
        {

            return new InstructionEjectTask();
        }
        
    }
    public abstract partial class Instruction
    {
        /// <summary>
        /// Field instruction task used above.
        /// </summary>
        protected class InstructionEjectTask : InstructionTask
        {
            protected override void OnRunStart()
            {
            }
        }
    }

}
