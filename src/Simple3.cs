using System;
using System.Collections.Generic;

namespace ver {

    public class Simple3 : IVersionType {

        private readonly Type[] typeFormat;
        public Type[] TypeFormat {get;}

        private List<object> data;
        public List<object> Data {get{return data;}}

        private int val1;
        private int val2;
        private int val3;

        public int Val1 {get {return val1;}}
        public int Val2 {get{return val2;}}
        public int Val3 {get{return val3;}}

        public Simple3(int val1, int val2, int val3) {
            typeFormat = new Type[3]{typeof(Int32), typeof(Int32), typeof(Int32)};
            this.val1 = val1;
            this.val2 = val2;
            this.val3 = val3;
            data = new List<object>() {Val1, Val2, Val3};
        }

        private bool calc(bool older, bool orEql, Simple3 input, bool onlyEql = false) {
            if (!orEql) {
                for (int i = 0; i < Data.Count; i++) {
                    if ((int)Data[i] > (int)input.Data[i]) {
                        return !older;
                    } else if ((int)Data[i] < (int)input.Data[i]) {
                        break;
                    }
                }
            } else {
                for (int i = 0; i < Data.Count; i++) {
                    if ((int)Data[i] == (int)input.Data[i]) {
                        return true;
                    }
                    if ((int)Data[i] > (int)input.Data[i]) {
                        return !older;
                    } else if ((int)Data[i] < (int)input.Data[i]) {
                        break;
                    }
                }
            }
            return older;
        }

        public bool isNewerThan(object input) {
            return calc(false, false, (Simple3)input);
        }

        public bool isOlderThan(object input) {
            return calc(true, false, (Simple3)input);
        }

        public bool isNewerThanOrEqualTo(object input) {
            return calc(false, true, (Simple3)input);
        }

        public bool isOlderThanOrEqualTo(object input) {
            return calc(true, true, (Simple3)input);
        }

        public bool isEqual(object input) {
            return calc(false, true, (Simple3)input, onlyEql:true);
        }

        public override string ToString() {
            return $"{Data[0]}.{Data[1]}.{Data[2]}";
        }

    }

}