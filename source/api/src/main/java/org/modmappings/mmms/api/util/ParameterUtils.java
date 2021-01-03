package org.modmappings.mmms.api.util;

public class ParameterUtils {

    private ParameterUtils() {
        throw new IllegalStateException("Can not instantiate an instance of: ParameterUtils. This is a utility class");
    }

    public static String toString(final Object arg) {
        if (arg == null)
            return "<?!?>NULL<?!?>";

        return arg.toString();
    }
}
