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
        static const AkUniqueID PAUSE = 3092587493U;
        static const AkUniqueID RESUME = 953277036U;
        static const AkUniqueID START_MUSIC = 540993415U;
        static const AkUniqueID STATE_CALM = 3675814650U;
        static const AkUniqueID STATE_INTENSE = 2575031991U;
        static const AkUniqueID STATE_MEDIATE = 1244834824U;
        static const AkUniqueID STATE_SILENT = 2880738500U;
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
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace PLAYING

    } // namespace STATES

    namespace SWITCHES
    {
        namespace LAUGH_METER
        {
            static const AkUniqueID GROUP = 2133615622U;

            namespace SWITCH
            {
                static const AkUniqueID A_FULL = 1892317246U;
                static const AkUniqueID B_NEAR_FULL = 4039310350U;
                static const AkUniqueID C_HALF = 2988341216U;
                static const AkUniqueID D_NEAR_EMPTY = 1959799710U;
            } // namespace SWITCH
        } // namespace LAUGH_METER

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID LAUGH_METER = 2133615622U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID EMPTY = 3354297748U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID STORY_FX = 2223886005U;
        static const AkUniqueID USER_INTERFACE = 3017890412U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
