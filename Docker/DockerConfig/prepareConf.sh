set -x
export tmpConfFile=/tmp/conf.$$

function replaceConf() {
  cp $confFile $tmpConfFile
  sed -e "s|$1|$2|" <$tmpConfFile  >$confFile 2>/tmp/sederrors
  if [ -s '/tmp/sederrors' ]
  then
    cat /tmp/sederrors
  fi
}

export confFile=/var/www/vhosts/backend/Web.config
replaceConf '###CONNECTION_STRING###' $CONNECTION_STRING
replaceConf '###DATA_SERVICE###' $DATA_SERVICE

#!/bin/sh
Xvfb -ac :99 -screen 0 1280x1024x16 &
export DISPLAY=:99
/bin/change_XMLconfig_from_env.sh
/bin/startApache.sh
