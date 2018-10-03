Vue.component("clash-clan-info",
    {
        props: {
            info: { type: Object },
            active: { type: Boolean, default: false }
        },
        template: `
<div class="block about" v-bind:class="{ active: active }">
    <div>
        <img v-bind:src="info.badgeUrls.small" alt="">
    </div>
    <div>
        <div>Всего очков: {{info.clanPoints}} / {{info.clanVersusPoints}}</div>
        <div>Серия побед: {{info.warWinStreak}}</div>
        <div>Выиграно войн: {{info.warWins}}</div>
        <div>Проиграно войн: {{info.warLosses}}</div>
        <div>Ничья: {{info.warTies}}</div>
        <div>Побед &divide; поражений: {{(info.warLosses === 0 ? 0 : info.warWins / info.warLosses).toFixed(3)}}</div>
        <div>Публичный варлог: {{info.isWarLogPublic ? "Да" : "Нет"}}</div>
        <div>Необходимо трофеев: {{info.requiredTrophies}}</div>
        <div>Расположение клана: {{info.location.name}}</div>
        <div>Соклановцы: {{info.members}}/50</div>
    </div>
</div>

`
    });

Vue.component("clash-clan-moto",
    {
        props: {
            description: {type: String},
            active: { type: Boolean, default: false }
        },
        template: `
<div class="block" v-bind:class="{ active: active }">
    <div>{{description}}</div>
</div>
`
    });

Vue.component("clash-clan-donation",
    {
        props: {
            clanDetails: { type: Object },
            active: { type: Boolean, default: false }
        },
        computed: {
            totalDonation: function () {
                return this.clanDetails.memberList.reduce((d, m) => m.donations + d, 0);
            },
            orderedMemberList: function () {
                return _.orderBy(this.clanDetails.memberList, "donations", "desc");
            }
        },
        template: `
<div class="block" v-bind:class="{ active: active }">
    Донат:
    <ol>
        <li class="user-details" v-for="item in orderedMemberList">
            <div style="font-size: larger;">
                <a href="https://link.clashofclans.com/?action=OpenPlayerProfile&tag=@member.Tag">
                    <img v-bind:src="item.league.iconUrls.small" width="25" height="25"/>
                    {{item.name}} ({{item.expLevel}})
                </a>
            </div>
            <div>
                <span>Пожертвовал: </span>{{item.donations}}
            </div>
            <div>
                <span>Получил: </span>{{item.donationsReceived}}
            </div>
            <div>
                <span>Вклад: </span>{{((item.donations / totalDonation)*100).toFixed(1)}}%
            </div>
        </li>
    </ol>
</div>
`
    });

Vue.component("clash-warlog-item",
    {
        props: {
            item: { type: Object }
        },
        computed: {
            background: function () {
                switch (this.item.result) {
                    case "win":
                        return "background-image: linear-gradient(rgba(128, 255, 128, 0.5), rgba(192, 255, 192, 0.8))";
                    case "lose":
                        return "background-image: linear-gradient(rgba(255, 128, 128, 0.5), rgba(255, 192, 192, 0.8))";
                    default :
                        return "background-image: linear-gradient(rgba(255, 255, 255, 0.3), rgba(255, 255, 255, 0.8))";
                }
            },
            orderedMemberList: function () {
                return _.orderBy(this.clanDetails.memberList, "donations", "desc");
            }
        },
        template: `
<div class="row" style="display: flex; flex-wrap: nowrap; white-space: nowrap" v-bind:style="background">
    <span style="flex: 1; text-align: end">
        <span style="font-size: 7px;margin-right: 5px">Optimus Gang</span>
        <span>{{item.clan.destructionPercentage.toFixed(2)}}%</span>
        <span>
            <img v-bind:src="item.clan.badgeUrls.small" width="22">
        </span>
    </span>
    <span style="flex:1">
        <span>
            <img v-bind:src="item.opponent.badgeUrls.small" width="22">
        </span>
        <span>{{item.opponent.destructionPercentage.toFixed(2)}}%</span>
        <span style="font-size: 7px;margin-left: 5px">{{item.opponent.name}}</span>
    </span>
</div>
`
    });

Vue.component("clash-warlog",
    {
        props: {
            warlog: { type: Object },
            active: { type: Boolean, default: false }
        },
        template: `
<div class="block" v-bind:class="{ active: active }">
    Ход войны:
    <ul>
        <li class="user-details" v-for="item in warlog.items">
            <clash-warlog-item v-bind:item="item"></clash-warlog-item>
        </li>
    </ul>
</div>
`
    });

var app = new Vue({
    el: "#app",
    created() {
        this.fetchData();
    },
    data() {
        return {
            clan: {
                warWins: 0,
                warLosses: 0,
                badgeUrls: { small: null },
                location: { name: null },
                description: ""
            },
            warlog: {
            },
            clanInfoActive: false,
            warlogActive: false
        }
    },
    methods: {
        fetchData() {
            fetch("api/clash/clan").then(r => r.json().then(t => {
                this.clan = t;
                this.clanInfoActive = true;
            }));

            fetch("api/clash/warlog").then(r => r.json().then(t => {
                this.warlog = t;
                this.warlogActive = true;
            }));
        }
    },
    template: `
<div>
<clash-clan-info v-bind:info="clan" v-bind:active="clanInfoActive"></clash-clan-info>
<clash-clan-moto v-bind:description="clan.description" v-bind:active="clanInfoActive"></clash-clan-moto>
<clash-clan-donation v-bind:clanDetails="clan" v-bind:active="clanInfoActive"></clash-clan-donation>
<clash-warlog  v-bind:warlog="warlog" v-bind:active="warlogActive"></clash-warlog>
</div>
`
})