using System;
using System.Collections.Generic;

namespace ver {

    public interface IVersionType {

        Type[] TypeFormat {get;}
        List<object> Data {get;}

        bool isNewerThan(object input);
        bool isOlderThan(object input);

    }

}