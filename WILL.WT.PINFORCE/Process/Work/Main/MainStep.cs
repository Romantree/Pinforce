namespace WILL.WT.PINFORCE.Process.Work.Main
{
    public enum MainStep
    {
        START,

        // AUTO CONTACT
        AUTO_CONTACT_CHECK,

        MOT_AUTO_CONTECT_READY_MOVE_ENTER,
        MOT_AUTO_CONTECT_READY_MOVE_POLLING,

        AUTO_CONTACT_COUNT_SETTTING,

        MOT_AUTO_CONTACT_01_MOVE_ENTER,
        MOT_AUTO_CONTACT_01_MOVE_POLLING,
        MOT_AUTO_CONTACT_01_DELAY,

        MOT_AUTO_CONTACT_02_MOVE_ENTER,
        MOT_AUTO_CONTACT_02_MOVE_POLLING,
        MOT_AUTO_CONTACT_02_DELAY,

        MOT_AUTO_CONTACT_03_MOVE_ENTER,
        MOT_AUTO_CONTACT_03_MOVE_POLLING,
        MOT_AUTO_CONTACT_03_DELAY,

        AUTO_CONTACT_CONUT_CHECK,

        LAST_RELEASE_CHECK,

        MOT_LAST_RELEASE_MOVE_ENTER,
        MOT_LAST_RELEASE_MOVE_POLLING,

        // AUTO CONTACT END

        // PINFORCE START
        MAIN_START,

        MOT_OD_MOVE_ENTER, // Step 이송
        MOT_OD_MOVE_POLLING, // Step 이송 대기
        MOT_OD_MOVE_DELAY, // Step 이송 딜레이

        MOT_OD_SAMPLING_START,
        MOT_OD_SAMPLING_DELAY, // Sampling 딜레이
        MOT_OD_CONTACT_START,
        MOT_OD_CONTACT_DELAY, // Contact Time 딜레이 -> Excel Output
        
        ADDLOG, // OUTPUT
        INC_STEP, // Step WorkCount 변경
        INC_OD, // OD Step 단계 변경
        INC_CNT, // PinForce WorkCount 변경
        CHECK_REPEAT_MODE, // 1 or 2

        MOT_RETURN_MOVE_ENTER, // 복귀이송
        MOT_RETURN_MOVE_POLLING, // 복귀이송대기
        MOT_RETURN_MOVE_DELAY, // 복귀이송 딜레이
        MOT_CHECK_REPEATCOUNT, // 반복횟수 체크

        CHECK_RELEASE_USE, // 완료 후 Release Use 여부
        RELEASE_USE_MOVE_ENTER, // 완료 이송
        RELEASE_USE_MOVE_POLLING, // 완료 이송 대기

        STOP_CAMERA,
        SAVE_DATA,
        END,
    }
}
