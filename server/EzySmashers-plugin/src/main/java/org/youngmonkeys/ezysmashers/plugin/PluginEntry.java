package org.youngmonkeys.ezysmashers.plugin;

import com.tvd12.ezydata.database.EzyDatabaseContext;
import com.tvd12.ezydata.jpa.EzyJpaDatabaseContextBuilder;
import com.tvd12.ezydata.jpa.loader.EzyJpaDataSourceLoader;
import com.tvd12.ezydata.jpa.loader.EzyJpaEntityManagerFactoryLoader;
import com.tvd12.ezyfox.bean.EzyBeanContextBuilder;
import com.tvd12.ezyfox.bean.impl.EzyBeanNameParser;
import com.tvd12.ezyfoxserver.context.EzyPluginContext;
import com.tvd12.ezyfoxserver.context.EzyZoneContext;
import com.tvd12.ezyfoxserver.setting.EzyPluginSetting;
import com.tvd12.ezyfoxserver.support.entry.EzyDefaultPluginEntry;

import javax.persistence.EntityManagerFactory;
import javax.sql.DataSource;
import java.util.Properties;

public class PluginEntry extends EzyDefaultPluginEntry {

    @Override
    protected void preConfig(EzyPluginContext ctx) {
        logger.info("\n=================== EzySmashers PLUGIN START CONFIG ================\n");
    }

    @Override
    protected void postConfig(EzyPluginContext ctx) {
        logger.info("\n=================== EzySmashers PLUGIN END CONFIG ================\n");
    }

    @Override
    protected void setupBeanContext(
        EzyPluginContext context,
        EzyBeanContextBuilder builder
    ) {
        EzyPluginSetting setting = context.getPlugin().getSetting();
        builder.addProperties("EzySmashers-common-config.properties");
        Properties properties = builder.getProperties();
        EzyDatabaseContext databaseContext = databaseContext(properties);
        databaseContext.getRepositories().forEach((repoType, repo) -> {
            builder.addSingleton(EzyBeanNameParser.getBeanName(repoType), repo);
        });
        EzyZoneContext zoneContext = context.getParent();
        zoneContext.setProperty("pluginProperties", properties);
        zoneContext.setProperty(EzyDatabaseContext.class, databaseContext);
    }

    private EzyDatabaseContext databaseContext(Properties properties) {
        return new EzyJpaDatabaseContextBuilder()
            .properties(properties)
            .entityManagerFactory(entityManagerFactory(properties))
            .scan("org.youngmonkeys.ezysmashers")
            .build();
    }

    private EntityManagerFactory entityManagerFactory(Properties properties) {
        return new EzyJpaEntityManagerFactoryLoader()
            .entityPackage("org.youngmonkeys.ezysmashers")
            .dataSource(dataSource(properties))
            .properties(properties)
            .load("Default");
    }

    private DataSource dataSource(Properties properties) {
        return new EzyJpaDataSourceLoader()
            .properties(properties, "datasource")
            .load();
    }

    @Override
    protected String[] getScanablePackages() {
        return new String[]{
            "org.youngmonkeys.ezysmashers.common",
            "org.youngmonkeys.ezysmashers.plugin"
        };
    }
}
