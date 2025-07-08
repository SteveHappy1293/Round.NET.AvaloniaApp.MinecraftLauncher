package SystemHelper;

import java.nio.file.Path;
import java.nio.file.Paths;

public class GetConfigPath {
    public static Path getConfigDir(String appName) {
        String os = System.getProperty("os.name").toLowerCase();

        if (os.contains("win")) {
            // Windows: %APPDATA%\appName
            return Paths.get(System.getenv("APPDATA"), appName);
        } else if (os.contains("mac")) {
            // macOS: ~/Library/Application Support/appName
            return Paths.get(System.getProperty("user.home"), "Library", "Application Support", appName);
        } else {
            // Linux/Unix: ~/.config/appName 或 $XDG_CONFIG_HOME/appName
            String xdgConfigHome = System.getenv("XDG_CONFIG_HOME");
            if (xdgConfigHome == null || xdgConfigHome.isEmpty()) {
                return Paths.get(System.getProperty("user.home"), ".config", appName);
            } else {
                return Paths.get(xdgConfigHome, appName);
            }
        }
    }
}
