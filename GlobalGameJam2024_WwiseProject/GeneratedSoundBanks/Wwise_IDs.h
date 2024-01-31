/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID GAME_OVER = 1432716332U;
        static const AkUniqueID MENU_ADJUST = 2166671746U;
        static const AkUniqueID MENU_BACK = 3063554414U;
        static const AkUniqueID MENU_NAV = 2684797902U;
        static const AkUniqueID MENU_SELECT = 4203375351U;
        static const AkUniqueID NPC_ANGRY = 1093725892U;
        static const AkUniqueID NPC_STOP = 2196625209U;
        static const AkUniqueID NPC_TALK = 1510544299U;
        static const AkUniqueID NPC_TIRED = 3425439425U;
        static const AkUniqueID PAUSE = 3092587493U;
        static const AkUniqueID QTE_HIT = 1217129213U;
        static const AkUniqueID QTE_MISS = 3855974658U;
        static const AkUniqueID QTE_PERFECT = 1426186273U;
        static const AkUniqueID RESUME = 953277036U;
        static const AkUniqueID START_MENU = 2977420043U;
        static const AkUniqueID START_MUSIC = 540993415U;
        static const AkUniqueID STATE_CALM = 3675814650U;
        static const AkUniqueID STATE_INTENSE = 2575031991U;
        static const AkUniqueID STATE_MEDIATE = 1244834824U;
        static const AkUniqueID STATE_SILENT = 2880738500U;
        static const AkUniqueID STOP_ALL = 452547817U;
        static const AkUniqueID STOP_MENU = 2914981333U;
        static const AkUniqueID STOP_MUSIC = 2837384057U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace INTENSITY
        {
            static const AkUniqueID GROUP = 2470328564U;

            namespace STATE
            {
                static const AkUniqueID CALM = 3753286132U;
                static const AkUniqueID INTENSE = 4223512837U;
                static const AkUniqueID MEDIATE = 4019181194U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SILENT = 3160623154U;
            } // namespace STATE
        } // namespace INTENSITY

        namespace PLAYING
        {
            static const AkUniqueID GROUP = 1852808225U;

            namespace STATE
            {
                static const AkUniqueID GAMEPLAY = 89505537U;
                static const AkUniqueID MENU = 2607556080U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace PLAYING

    } // namespace STATES

    namespace SWITCHES
    {
        namespace NPC_DIALOGUE
        {
            static const AkUniqueID GROUP = 1541636155U;

            namespace SWITCH
            {
                static const AkUniqueID ANGRY = 1206605712U;
                static const AkUniqueID TIRED = 3386657621U;
            } // namespace SWITCH
        } // namespace NPC_DIALOGUE

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID LAUGHTER = 1097468465U;
        static const AkUniqueID MASTERVOLUME = 2918011349U;
        static const AkUniqueID MUSICVOLUME = 2346531308U;
        static const AkUniqueID SOUNDVOLUME = 3873835272U;
        static const AkUniqueID VOICEVOLUME = 414646191U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID EMPTY = 3354297748U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID DIALOGUE = 3930136735U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX = 393239870U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
