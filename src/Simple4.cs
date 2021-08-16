using System;
using System.Collections.Generic;

namespace ver {

    public class Simple4 : IVersionType {

        private readonly Type[] typeFormat;
        public Type[] TypeFormat {get;}

        private List<object> data;
        public List<object> Data {get{return data;}}

        private int val1;
        private int val2;
        private int val3;
        private int val4;

        public int Val1 {get {return val1;}}
        public int Val2 {get{return val2;}}
        public int Val3 {get{return val3;}}
        public int Val4 {get{return val4;}}

        private bool rv;
        public bool Rv {get{return rv;}}

        public Simple4(int val1, int val2, int val3, int val4, bool rv = false) {
            typeFormat = new Type[4]{typeof(Int32), typeof(Int32), typeof(Int32), typeof(Int32)};
            this.val1 = val1;
            this.val2 = val2;
            this.val3 = val3;
            this.val4 = val4;
            this.rv = rv;
            data = new List<object>() {Val1, Val2, Val3, Val4};
        }

        private bool calc(bool older, Simple4 input) {
            for (int i = 0; i < Data.Count; i++) {
                if ((int)Data[i] > (int)input.Data[i]) {
                    return !older;
                } else if ((int)Data[i] < (int)input.Data[i]) {
                    break;
                }
            }
            return older;
        }

        public bool isNewerThan(object input) {
            return calc(false, (Simple4)input);
        }

        public bool isOlderThan(object input) {
            return calc(true, (Simple4)input);
        }

        public override string ToString() {
            if (Rv) {
                return $"{Data[0]}.{Data[1]}.{Data[2]}rv{Data[3]}";
            } else {
                return $"{Data[0]}.{Data[1]}.{Data[2]}.{Data[3]}";
            }
        }

    }

}