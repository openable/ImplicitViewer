using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.IO;

namespace ImplicitViewer.Model
{
    class Setting
    {
        public static StreamReader reader;
        public static string path;
        public static Encoding encode;

        public static string ID;
        public static int fontSize;
        public static int SCREEN_WIDTH;
        public static int SCREEN_HEIGHT;

        public static Point margin;         // 스크린 양끝 기본 여백 지정 변수
        public static PointF sStimulus;     // Stimulus 버튼 Width, Height 크기(Size) 정보
        public static PointF cStimulus;     // Stimulus x, y 좌표(Coordinate) 정보
        public static PointF aWord;         // Choice 전체 면적 Width, Height 정보
        public static float xInterval;      // 열 5개 기준 Width 너비 여유 공간
        public static float yInterval;      // 행 3개 기준 Height 높이 여유 공간
        public static float xBuffer;        // 시선 인정 Width 너비 버퍼
        public static float yBuffer;        // 시선 인정 Height 높이 버퍼
        public static PointF sWord;         // Choice 버튼 Width, Height 크기(size) 정보
        public static PointF[] cWord = new PointF[15];  // Choice 15개의 개별 위치 미리 지정용

        public static Main main;
        public static ArrayList taskList;
    }
}
